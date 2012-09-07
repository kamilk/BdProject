using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using ReferenceArchiver.Model;
using ReferenceArchiver.ViewModel.Helpers;

namespace ReferenceArchiver.ViewModel
{
    class ChooseResearchJournalPageViewModel : WizardPageViewModelBase
    {
        #region Fields

        SearchableCollectionViewWrapper<ResearchJournal> _researchJournals;

        #endregion

        #region Bound properties

        public override string Title
        {
            get { return "Wybierz serię"; }
        }

        public ResearchJournal SelectedJournal
        { get { return ResearchJournals.CurrentItem as ResearchJournal; } }

        public ICollectionView ResearchJournals
        {
            get { return _researchJournals.CollectionView; }
        }

        public string JournalSearchString
        {
            get { return _researchJournals.SearchString; }
            set
            {
                _researchJournals.SearchString = value;
                NotifyPropertyChanged(JournalSearchString);
                ResearchJournals.Refresh();
            }
        }

        #endregion

        #region Constructors

        public ChooseResearchJournalPageViewModel(WizardViewModel parent)
            : base(parent)
        { }

        #endregion

        #region Methods

        public override void OnNavigatedTo(NavigationDirection direction)
        {
            base.OnNavigatedTo(direction);

            if (direction == NavigationDirection.Forward)
            {
                ICollectionView researchJournalsCollectionView = new CollectionViewSource
                {
                    Source = CentralRepository.Instance.GetJournalsForPublisher(WizardViewModel.SelectedPublisher).ToList()
                }.View;

                _researchJournals = new SearchableCollectionViewWrapper<ResearchJournal>(
                    researchJournalsCollectionView,
                    journal => journal.Title);
                NotifyPropertyChanged("ResearchJournals");
            }
        }

        #endregion
    }
}
