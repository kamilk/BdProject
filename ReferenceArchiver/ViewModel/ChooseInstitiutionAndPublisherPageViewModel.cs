using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BdGui2.Model;

namespace BdGui2.ViewModel
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
