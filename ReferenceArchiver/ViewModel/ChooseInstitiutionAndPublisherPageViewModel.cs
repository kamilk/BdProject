using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BdGui2.ViewModel
{
    class ChooseInstitiutionAndPublisherPageViewModel : WizardPageViewModelBase
    {
        public override string Title
        {
            get { return "Wybierz instytucję i wydawnictwo"; }
        }

        public ChooseInstitiutionAndPublisherPageViewModel(WizardViewModel parent)
            : base(parent)
        { }
    }
}
