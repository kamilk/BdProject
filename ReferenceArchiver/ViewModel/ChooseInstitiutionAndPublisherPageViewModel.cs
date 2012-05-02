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

        private IFilteredListProvider<Institution> _Institutions;
        public IFilteredListProvider<Institution> Institutions
        {
            get
            {
                return _Institutions;
            }
            set
            {
                _Institutions = value;
                NotifyPropertyChanged("Institutions");
            }
        }

        public ChooseInstitiutionAndPublisherPageViewModel(WizardViewModel parent)
            : base(parent)
        { }
    }
}
