﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using ReferenceArchiver.Model;

namespace ReferenceArchiver.ViewModel
{
    class WizardViewModel : INotifyPropertyChanged
    {
        ChooseInstitiutionAndPublisherPageViewModel _institutionView;
        ChooseResearchJournalPageViewModel _journalView;
        ChooseIssuePageViewModel _issueView;
        AddArticlePageViewModel _articleView;

        ChoosePublisherForReferencePageViewModel _referenceInstitutionView;
        ChooseResearchJournalPageViewModel _referenceJournalView;
        ChooseIssuePageViewModel _referenceIssueView;
        AddArticleWithoutReferencesViewModel _referenceArticleView;

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
                if (_institutionView.IsDataLockedIn)
                    return _institutionView.SelectedInstitution;
                else
                    return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public WizardViewModel()
        {
            _institutions = new List<Institution>(CentralRepository.Instance.GetInstitutions());
            _publishers = new List<Publisher>(CentralRepository.Instance.GetPublishers());

            _institutionView = new ChooseInstitiutionAndPublisherPageViewModel(this, _institutions, _publishers);
            _journalView = new ChooseResearchJournalPageViewModel(this);
            _issueView = new ChooseIssuePageViewModel(this);
            _articleView = new AddArticlePageViewModel(this);

            _mainPageManager = new WizardPageManager();
            _mainPageManager.Add(_institutionView);
            _mainPageManager.Add(_journalView);
            _mainPageManager.Add(_issueView);
            _mainPageManager.Add(_articleView);

            PageManager = _mainPageManager;

            _referenceInstitutionView = new ChoosePublisherForReferencePageViewModel(this, _institutions, _publishers);
            _referenceJournalView = new ChooseResearchJournalPageViewModel(this);
            _referenceIssueView = new ChooseIssuePageViewModel(this);
            _referenceArticleView = new AddArticleWithoutReferencesViewModel(this);

            _referencePageManager = new WizardPageManager();
            _referencePageManager.Add(_referenceInstitutionView);
            _referencePageManager.Add(_referenceJournalView);
            _referencePageManager.Add(_referenceIssueView);
            _referencePageManager.Add(_referenceArticleView);

            _referencePageManager.CanNavigateBeyondBeginning = true;
            _referencePageManager.BeginningReached += new EventHandler(_referencePageManager_BeginningReached);

            _institutionView.DataLockedInChanged += new EventHandler(InstitutionView_DataLockedInChanged);
        }

        void _referencePageManager_BeginningReached(object sender, EventArgs e)
        {
            PageManager = _mainPageManager;
        }

        protected void NotifyPropertyChanged(string parameterName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(parameterName));
        }

        internal void NavigateToAddingReferenceArticle()
        {
            PageManager = _referencePageManager;
        }

        void InstitutionView_DataLockedInChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("SelectedInstitution");
        }
    }
}