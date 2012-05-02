using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ReferenceArchiver.ViewModel
{
    class WizardControlViewModelBase : INotifyPropertyChanged
    {
        public WizardViewModel WizardViewModel
        {
            get;
            private set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public WizardControlViewModelBase(WizardViewModel parent)
        {
            WizardViewModel = parent;
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
