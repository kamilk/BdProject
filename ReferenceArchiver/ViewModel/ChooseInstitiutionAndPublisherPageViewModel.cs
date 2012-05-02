using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReferenceArchiver.Model;

namespace ReferenceArchiver.ViewModel
{
    class ChooseInstitiutionAndPublisherPageViewModel : WizardPageViewModelBase
    {
        public override string Title
        {
            get { return "Wybierz instytucję i wydawnictwo"; }
        }

        private IFilteredListProvider<Institution> _institutions;
        public IFilteredListProvider<Institution> Institutions
        {
            get
            {
                return _institutions;
            }
            set
            {
                _institutions = value;
                NotifyPropertyChanged("Institutions");
            }
        }

        private IFilteredListProvider<Publisher> _publishers;
        public IFilteredListProvider<Publisher> Publishers
        {
            get
            {
                return _publishers;
            }
            set
            {
                _publishers = value;
                NotifyPropertyChanged("Publishers");
            }
        }

        public ChooseInstitiutionAndPublisherPageViewModel(WizardViewModel parent)
            : base(parent)
        { }
    }
}
