using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ReferenceArchiver.ViewModel
{
    abstract class WizardPageViewModelBase : WizardControlViewModelBase
    {
        public abstract string Title
        {
            get;
        }

        public WizardPageViewModelBase(WizardViewModel parent)
            : base(parent)
        {}
    }
}
