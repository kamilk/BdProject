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
    /// <summary>
    /// A ViewModel for ChooseIssuePageView view.
    /// </summary>
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

        /// <summary>
        /// Gets or sets the selected issue.
        /// </summary>
        /// <value>
        /// The selected issue.
        /// </value>
        public Issue SelectedIssue
        {
            get { return _selectedIssue; }
            set
            {
                _selectedIssue = value;
                OnIssueChanged();
            }
        }

        /// <summary>
        /// Gets an ICollectionView exposing a list of issue types the user can choose from.
        /// </summary>
        /// <value>
        /// The issue types.
        /// </value>
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

        /// <summary>
        /// Gets or sets the type number.
        /// </summary>
        /// <value>
        /// The type number.
        /// </value>
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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChooseIssuePageViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public ChooseIssuePageViewModel(WizardViewModel parent)
            : base(parent)
        { }

        #endregion

        #region Methods

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
