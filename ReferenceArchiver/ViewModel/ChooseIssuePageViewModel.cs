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
        #region Fields

        private static readonly ICollection<IssueTypeWrapper> _issueTypeWrappers = IssueTypeWrapper.GetAllIssueTypes();
        private Issue _selectedIssue;
        private int? _numberWithinJournal;
        private int? _numberWithinPublisher;
        private ICollectionView _issueTypes;
        private DelegateCommand _searchByNumberWithinJournalCommand;

        #endregion

        #region Bound properties

        public override string Title
        {
            get { return "Wybierz zeszyt"; }
        }

        public Issue SelectedIssue 
        {
            get { return _selectedIssue; }
            private set
            {
                _selectedIssue = value;
                OnIssueChanged();
            }
        }

        public int? NumberWithinJournal
        {
            get { return _numberWithinJournal; }
            set
            {
                _numberWithinJournal = value;
                NotifyPropertyChanged("NumberWithinJournal");
            }
        }

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

        #endregion

        #region Constructors

        public ChooseIssuePageViewModel(WizardViewModel parent)
            : base(parent)
        { }

        #endregion

        #region Methods

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

        #endregion
    }
}
