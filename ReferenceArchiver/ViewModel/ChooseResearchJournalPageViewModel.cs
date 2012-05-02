using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.ViewModel
{
    class ChooseResearchJournalPageViewModel : WizardPageViewModelBase
    {
        public override string Title
        {
            get { return "Wybierz serię"; }
        }

        public ChooseResearchJournalPageViewModel(WizardViewModel parent)
            : base(parent)
        { }
    }
}
