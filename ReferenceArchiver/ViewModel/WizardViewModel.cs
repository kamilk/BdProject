using System;
using System.Collections.Generic;
using System.ComponentModel;
using ReferenceArchiver.Model;
using ReferenceArchiver.ViewModel.Helpers;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;
using ReferenceArchiver.Invenio;

namespace ReferenceArchiver.ViewModel
{
    /// <summary>
    /// Main ViewModel class for MainWindow view.
    /// </summary>
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

        /// <summary>
        /// Gets or sets the page manager.
        /// </summary>
        /// <value>
        /// The page manager.
        /// </value>
        public WizardPageManager PageManager
        {
            get { return _pageManager; }
            set
            {
                _pageManager = value;
                NotifyPropertyChanged("PageManager");
            }
        }

        /// <summary>
        /// Gets the institution selected by the user, or null if none has been selected yet.
        /// </summary>
        /// <value>
        /// The selected institution.
        /// </value>
        public Institution SelectedInstitution
        {
            get
            {
                return _institutionView.IsDataLockedIn ? _institutionView.SelectedInstitution : null;
            }
        }

        /// <summary>
        /// Gets the publisher selected by the user, or null if none has been selected yet.
        /// </summary>
        /// <value>
        /// The selected publisher.
        /// </value>
        public Publisher SelectedPublisher
        {
            get
            {
                return _institutionView.IsDataLockedIn ? _institutionView.SelectedPublisher : null;
            }
        }

        /// <summary>
        /// Gets the research journal selected by the user, or null if none has been selected yet.
        /// </summary>
        /// <value>
        /// The selected journal.
        /// </value>
        public ResearchJournal SelectedJournal
        { get { return _journalView.IsDataLockedIn ? _journalView.SelectedJournal : null; } }

        DelegateCommand _navigateForwardCommand;

        /// <summary>
        /// Gets the command which should be executed when the user makes a request to go the next
        /// page of the wizard.
        /// </summary>
        /// <value>
        /// The navigate forward command.
        /// </value>
        public ICommand NavigateForwardCommand
        {
            get
            {
                if (_navigateForwardCommand == null)
                    _navigateForwardCommand = new DelegateCommand(PageManager.NavigateForward);
                return _navigateForwardCommand;
            }
        }

        DelegateCommand _navigateBackwardCommand;

        /// <summary>
        /// Gets the command which should be executed when the user makes a request to go the previous
        /// page of the wizard.
        /// </summary>
        /// <value>
        /// The navigate backward command.
        /// </value>
        public ICommand NavigateBackwardCommand
        {
            get
            {
                if (_navigateBackwardCommand == null)
                    _navigateBackwardCommand = new DelegateCommand(PageManager.NavigateBackward);
                return _navigateBackwardCommand;
            }
        }

        DelegateCommand _navigateToAddingReferenceArticleCommand;

        /// <summary>
        /// Gets the command which should be executed when the user makes a request to go the screen
        /// for adding an article for the purpose of including it in the currently edited article's
        /// references.
        /// </summary>
        /// <value>
        /// The navigate to adding reference article command.
        /// </value>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardViewModel" /> class.
        /// </summary>
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

            _mainPageManager.AllowNavigationBeyondEnd = true;
            _mainPageManager.EndReached += new EventHandler(MainPageManager_EndReached);

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

            _referencePageManager.AllowNavigationBeyondBeginning = true;
            _referencePageManager.BeginningReached += new EventHandler(ReferencePageManager_BeginningReached);

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

        private void ReferencePageManager_BeginningReached(object sender, EventArgs e)
        {
            PageManager = _mainPageManager;
        }

        private void InstitutionView_DataLockedInChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("SelectedInstitution");
            NotifyPropertyChanged("SelectedPublisher");
        }

        private void InstitutionView_IsDataCompleteChanged(object sender, EventArgs e)
        {
            PageManager.RefreshCanNavigate();
        }

        private void MainPageManager_EndReached(object sender, EventArgs e)
        {
            Article article = _articleView.GetArticle();
            article.SetIssue(_issueView.SelectedIssue);
            CentralRepository.Instance.SaveArticleWithAuthorshipsAndReferences(
                article, _articleView.GetAuthorships(), _articleView.GetReferences());

            if (_articleView.ShouldExportToInvenio)
            {
                var dialog = new SaveFileDialog();
                dialog.Title = "Wybierz lokalizację dla pliku Invenio";
                dialog.DefaultExt = ".xml";
                dialog.Filter = "XML Documents (.xml)|*.xml";

                if (dialog.ShowDialog() == true)
                    new Marcxml().GenerateXml(article, dialog.FileName);
            }
        }
    }
}
