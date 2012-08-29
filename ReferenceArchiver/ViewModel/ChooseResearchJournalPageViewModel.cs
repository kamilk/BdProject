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

        SearchableCollectionViewWrapper<ResearchJournal> _researchJournals;
        public ICollectionView ResearchJournals
        {
            get { return _researchJournals.CollectionView; }
        }

        public ChooseResearchJournalPageViewModel(WizardViewModel parent)
            : base(parent)
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
