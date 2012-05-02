using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.ViewModel
{
    class AddArticlePageViewModel : AddArticleWithoutReferencesViewModel
    {
        public AddArticlePageViewModel(WizardViewModel parent)
            : base(parent)
        { }
    }
}
