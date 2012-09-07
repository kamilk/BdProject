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
        public override string Title
        {
            get { return "Wybierz serię"; }
        }

        public ResearchJournal SelectedJournal
        { get { return ResearchJournals.CurrentItem as ResearchJournal; } }

        SearchableCollectionViewWrapper<ResearchJournal> _researchJournals;
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

        public ChooseResearchJournalPageViewModel(WizardViewModel parent)
            : base(parent)
        { }

        public override void OnNavigatedTo(NavigationDirection direction)
        {
            base.OnNavigatedTo(direction);

            if (direction == NavigationDirection.Forward)
            {
                ICollectionView researchJournalsCollectionView = new CollectionViewSource
                {
                    Source = CentralRepository.Instance.GetJournalsForPublisher(CentralRepository.Instance.GetPublishers().FirstOrDefault()).ToList()
                }.View;

                _researchJournals = new SearchableCollectionViewWrapper<ResearchJournal>(
                    researchJournalsCollectionView,
                    journal => journal.Title);
            }
        }
    }
}
