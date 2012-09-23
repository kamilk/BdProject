using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.ViewModel
{
    class AddArticleViewModel : WizardPageViewModelBase
    {
        public override string Title
        {
            get { return "Podaj dane artykułu"; }
        }

        public AddArticleViewModel(WizardViewModel parent)
            : base(parent)
        { }
    }
}
