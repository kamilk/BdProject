﻿using System;
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

        public WizardPageViewModelBase(WizardViewModel parent)
            : base(parent)
        {
            IsDataLockedIn = false;
        }

        public void OnNavigatedTo()
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
    }
}