using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ReferenceArchiver.Model;
using ReferenceArchiver.ViewModel.Helpers;

namespace ReferenceArchiver.ViewModel
{
    class ChooseIssuePageViewModel : WizardPageViewModelBase
    {
        private static readonly ICollection<IssueTypeWrapper> _issueTypeWrappers = IssueTypeWrapper.GetAllIssueTypes();

        public override string Title
        {
            get { return "Wybierz zeszyt"; }
        }

        private Issue _selectedIssue;
        public Issue SelectedIssue 
        {
            get { return _selectedIssue; }
            private set
            {
                _selectedIssue = value;
                OnIssueChanged();
            }
        }

        private int? _numberWithinJournal;
        public int? NumberWithinJournal
        {
            get { return _numberWithinJournal; }
            set
            {
                _numberWithinJournal = value;
                NotifyPropertyChanged("NumberWithinJournal");
            }
        }

        private int? _numberWithinPublisher;
        public int? NumberWithinPublisher
        {
            get { return _numberWithinPublisher; }
            set
            {
                _numberWithinPublisher = value;
                NotifyPropertyChanged("NumberWithinPublisher");
            }
        }

        public string IssueTitle
        {
            get { return _selectedIssue == null ? string.Empty : _selectedIssue.Title; }
            set
            {
                if (_selectedIssue != null || value != null)
                {
                    if (_selectedIssue == null)
                        _selectedIssue = new Issue(WizardViewModel.SelectedJournal);
                    _selectedIssue.Title = value;
                }

                NotifyPropertyChanged("IssueTitle");
            }
        }

        public int? YearOfPublication 
        {
            get { return _selectedIssue == null ? null : _selectedIssue.YearOfPublication; }
            set
            {
                if (_selectedIssue != null || value != null)
                {
                    if (_selectedIssue == null)
                        _selectedIssue = new Issue(WizardViewModel.SelectedJournal);
                    _selectedIssue.YearOfPublication = value;
                }

                NotifyPropertyChanged("YearOfPublication");
            }
        }

        private ICollectionView _issueTypes;
        public ICollectionView IssueTypes
        {
            get
            {
                if (_issueTypes == null)
                {
                    _issueTypes = CollectionViewSource.GetDefaultView(_issueTypeWrappers);
                    _issueTypes.CurrentChanged += new EventHandler(IssueTypes_CurrentChanged);
                }
                return _issueTypes;
            }
        }

        public string TypeNumber
        {
            get { return _selectedIssue == null ? string.Empty : _selectedIssue.TypeNumber; }
            set
            {
                if (_selectedIssue == null && value != null)
                    _selectedIssue = new Issue(WizardViewModel.SelectedJournal);
                _selectedIssue.Title = value;
                NotifyPropertyChanged("IssueTitle");
            }
        }

        private DelegateCommand _searchByNumberWithinJournalCommand;
        public ICommand SearchByNumberWithinJournalCommand
        {
            get
            {
                if (_searchByNumberWithinJournalCommand == null)
                {
                    _searchByNumberWithinJournalCommand = new DelegateCommand(() =>
                    {
                        Issue issue = null;
                        if (NumberWithinJournal != null)
                        {
                            issue = CentralRepository.Instance.GetIssueByNumberWithinJournal(
                                 WizardViewModel.SelectedJournal, (int)NumberWithinJournal);
                        }
                        SelectedIssue = issue;
                        FillIssueData(issue);
                    });
                }

                return _searchByNumberWithinJournalCommand;
            }
        }

        public ChooseIssuePageViewModel(WizardViewModel parent)
            : base(parent)
        { }

        private void FillIssueData(Issue issue)
        {
            if (issue == null)
            {
                NumberWithinJournal = null;
                NumberWithinPublisher = null;
            }
            else
            {
                NumberWithinJournal = issue.NumberWithinJournal;
                NumberWithinPublisher = issue.NumberWithinPublisher;
            }

            OnIssueChanged();
        }

        private void OnIssueChanged()
        {
            NotifyPropertyChanged("IssueTitle");
            NotifyPropertyChanged("YearOfPublication");
            NotifyPropertyChanged("TypeNumber");

            IssueTypeWrapper issueTypeToSelect = SelectedIssue == null ? null 
                : _issueTypeWrappers.Where(i => i.Value == SelectedIssue.Type).FirstOrDefault();
            IssueTypes.MoveCurrentTo(issueTypeToSelect);
        }

        void IssueTypes_CurrentChanged(object sender, EventArgs e)
        {
            var issueTypeWrapper = IssueTypes.CurrentItem as IssueTypeWrapper;
            if (issueTypeWrapper != null)
            {
                if (_selectedIssue == null)
                    _selectedIssue = new Issue(WizardViewModel.SelectedJournal);
                _selectedIssue.Type = issueTypeWrapper.Value;
            }
        }
    }
}
