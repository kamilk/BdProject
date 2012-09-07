using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ReferenceArchiver.ViewModel.Helpers;

namespace ReferenceArchiver.ViewModel
{
    abstract class WizardPageViewModelBase : WizardControlViewModelBase
    {
        public abstract string Title
        {
            get;
        }

        private bool _isDataLockedIn;
        public bool IsDataLockedIn
        {
            get
            {
                return _isDataLockedIn;
            }
            protected set
            {
                _isDataLockedIn = value;
                NotifyPropertyChanged("IsDataLockedIn");
                if (DataLockedInChanged != null)
                    DataLockedInChanged(this, new EventArgs());
            }
        }

        public event EventHandler DataLockedInChanged;
        public event EventHandler IsDataCompleteChanged;

        public WizardPageViewModelBase(WizardViewModel parent)
            : base(parent)
        {
            IsDataLockedIn = false;
        }

        public virtual void OnNavigatedTo(NavigationDirection direction)
        {
            IsDataLockedIn = false;
        }

        public void OnNavigatedFromForward()
        {
            IsDataLockedIn = true;
        }

        public void OnNavigatedFromBackward()
        {
            IsDataLockedIn = false;
        }

        public virtual bool IsDataComplete
        { get { return true; } }

        protected void NotifyIsDataCompleteChanged()
        {
            if (IsDataCompleteChanged != null)
                IsDataCompleteChanged(this, new EventArgs());
        }
    }
}
