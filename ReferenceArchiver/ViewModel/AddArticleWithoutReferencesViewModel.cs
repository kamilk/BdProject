﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BdGui2.ViewModel
{
    class AddArticleWithoutReferencesViewModel : WizardPageViewModelBase
    {
        public override string Title
        {
            get { return "Podaj dane artykułu"; }
        }

        public AddArticleWithoutReferencesViewModel(WizardViewModel parent)
            : base(parent)
        { }
    }
}
