using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using ReferenceArchiver.Model;
using ReferenceArchiver.ViewModel.Helpers;
using System.Collections.ObjectModel;

namespace ReferenceArchiver.ViewModel
{
    class ChooseResearchJournalPageViewModel : WizardPageViewModelBase
    {
        #region Fields

        SearchableCollectionViewWrapper<ResearchJournal> _researchJournals;
        ObservableCollection<ResearchJournal> _journalsCollection;

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
                _journalsCollection = new ObservableCollection<ResearchJournal>(
                    CentralRepository.Instance.GetJournalsForPublisher(WizardViewModel.SelectedPublisher));

                ICollectionView researchJournalsCollectionView = new CollectionViewSource
                {
                    Source = _journalsCollection
                }.View;

                _researchJournals = new SearchableCollectionViewWrapper<ResearchJournal>(
                    researchJournalsCollectionView,
                    journal => journal.Title);
                NotifyPropertyChanged("ResearchJournals");
            }
        }

        public void AddAndSelectJournal(ResearchJournal journal)
        {
            _journalsCollection.Add(journal);
            _researchJournals.CollectionView.MoveCurrentTo(journal);
        }

        #endregion
    }
}
