using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using ReferenceArchiver.Model;

namespace ReferenceArchiver.ViewModel
{
    class ChooseResearchJournalPageViewModel : WizardPageViewModelBase
    {
        public override string Title
        {
            get { return "Wybierz serię"; }
        }

        ICollectionView _researchJournals;
        public ICollectionView ResearchJournals
        {
            get { return _researchJournals; }
            set
            {
                _researchJournals = value;
                NotifyPropertyChanged("ResearchJournals");
            }
        }

        public ChooseResearchJournalPageViewModel(WizardViewModel parent)
            : base(parent)
        {
            ResearchJournals = new CollectionViewSource 
            {
                Source = CentralRepository.Instance.GetJournalsForPublisher(CentralRepository.Instance.GetPublishers().FirstOrDefault()).ToList() 
            }.View;
        }
    }
}
