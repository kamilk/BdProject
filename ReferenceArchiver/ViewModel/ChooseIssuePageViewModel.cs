using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.ViewModel
{
    class ChooseIssuePageViewModel : WizardPageViewModelBase
    {
        public override string Title
        {
            get { return "Wybierz zeszyt"; }
        }

        public ChooseIssuePageViewModel(WizardViewModel parent)
            : base(parent)
        { }
    }
}
