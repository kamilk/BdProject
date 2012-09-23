using System;
using System.Collections.Generic;
using System.ComponentModel;
using ReferenceArchiver.Model;
using ReferenceArchiver.ViewModel.Helpers;
using System.Windows.Input;

namespace ReferenceArchiver.ViewModel
{
    class WizardViewModel : INotifyPropertyChanged
    {
        ChooseInstitiutionAndPublisherPageViewModel _institutionView;
        ChooseResearchJournalPageViewModel _journalView;
        ChooseIssuePageViewModel _issueView;
        AddArticleWithReferencesPageViewModel _articleView;

        ChoosePublisherForReferencePageViewModel _referenceInstitutionView;
        ChooseResearchJournalPageViewModel _referenceJournalView;
        ChooseIssuePageViewModel _referenceIssueView;
        AddArticleViewModel _referenceArticleView;

        WizardPageManager _mainPageManager;
        WizardPageManager _referencePageManager;

        List<Institution> _institutions;
        List<Publisher> _publishers;

        private WizardPageManager _pageManager;
        public WizardPageManager PageManager
        {
            get
            {
                return _pageManager;
            }
            set
            {
                _pageManager = value;
                NotifyPropertyChanged("PageManager");
            }
        }

        public Institution SelectedInstitution
        {
            get
            {
                return _institutionView.IsDataLockedIn ? _institutionView.SelectedInstitution : null;
            }
        }

        public Publisher SelectedPublisher
        {
            get
            {
                return _institutionView.IsDataLockedIn ? _institutionView.SelectedPublisher : null;
            }
        }

        public ResearchJournal SelectedJournal
        { get { return _journalView.IsDataLockedIn ? _journalView.SelectedJournal : null; } }

        DelegateCommand _navigateForwardCommand;
        public ICommand NavigateForwardCommand
        {
            get
            {
                if (_navigateForwardCommand == null)
                    _navigateForwardCommand = new DelegateCommand(_pageManager.NavigateForward);
                return _navigateForwardCommand;
            }
        }

        DelegateCommand _navigateBackwardCommand;
        public ICommand NavigateBackwardCommand
        {
            get
            {
                if (_navigateBackwardCommand == null)
                    _navigateBackwardCommand = new DelegateCommand(_pageManager.NavigateBackward);
                return _navigateBackwardCommand;
            }
        }

        DelegateCommand _navigateToAddingReferenceArticleCommand;
        public ICommand NavigateToAddingReferenceArticleCommand
        {
            get
            {
                if (_navigateToAddingReferenceArticleCommand == null)
                    _navigateToAddingReferenceArticleCommand = new DelegateCommand(NavigateToAddingReferenceArticle);
                return _navigateToAddingReferenceArticleCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public WizardViewModel()
        {
            _institutions = new List<Institution>(CentralRepository.Instance.GetInstitutions());
            _institutions.Sort((x, y) => x.Name.CompareTo(y.Name));
            _publishers = new List<Publisher>(CentralRepository.Instance.GetPublishers());
            _publishers.Sort((x, y) => x.Title.CompareTo(y.Title));
            _institutionView = new ChooseInstitiutionAndPublisherPageViewModel(this, _institutions, _publishers);
            _journalView = new ChooseResearchJournalPageViewModel(this);
            _issueView = new ChooseIssuePageViewModel(this);
            _articleView = new AddArticleWithReferencesPageViewModel(this);

            _mainPageManager = new WizardPageManager();
            _mainPageManager.Add(_institutionView);
            _mainPageManager.Add(_journalView);
            _mainPageManager.Add(_issueView);
            _mainPageManager.Add(_articleView);

            PageManager = _mainPageManager;

            _referenceInstitutionView = new ChoosePublisherForReferencePageViewModel(this, _institutions, _publishers);
            _referenceJournalView = new ChooseResearchJournalPageViewModel(this);
            _referenceIssueView = new ChooseIssuePageViewModel(this);
            _referenceArticleView = new AddArticleViewModel(this);

            _referencePageManager = new WizardPageManager();
            _referencePageManager.Add(_referenceInstitutionView);
            _referencePageManager.Add(_referenceJournalView);
            _referencePageManager.Add(_referenceIssueView);
            _referencePageManager.Add(_referenceArticleView);

            _referencePageManager.CanNavigateBeyondBeginning = true;
            _referencePageManager.BeginningReached += new EventHandler(_referencePageManager_BeginningReached);

            _institutionView.DataLockedInChanged += new EventHandler(InstitutionView_DataLockedInChanged);
            _institutionView.IsDataCompleteChanged += new EventHandler(InstitutionView_IsDataCompleteChanged);
        }

        private void NavigateToAddingReferenceArticle()
        {
            PageManager = _referencePageManager;
        }

        protected void NotifyPropertyChanged(string parameterName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(parameterName));
        }

        private void _referencePageManager_BeginningReached(object sender, EventArgs e)
        {
            PageManager = _mainPageManager;
        }

        private void InstitutionView_DataLockedInChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("SelectedInstitution");
            NotifyPropertyChanged("SelectedPublisher");
        }

        void InstitutionView_IsDataCompleteChanged(object sender, EventArgs e)
        {
            PageManager.RefreshCanNavigate();
        }
    }
}
