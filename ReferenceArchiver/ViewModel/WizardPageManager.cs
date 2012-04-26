using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace BdGui2.ViewModel
{
    class WizardPageManager : INotifyPropertyChanged
    {
        List<WizardPageViewModelBase> _pages = new List<WizardPageViewModelBase>();

        int currentPage = 0;

        WizardPageViewModelBase _currentPage;
        public WizardPageViewModelBase CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                NotifyPropertyChanged("CurrentPage");
            }
        }

        public bool CanNavigateBeyondBeginning { get; set; }
        public bool CanNavigateBeyondEnd { get; set; }

        public bool CanNavigateForward
        {
            get
            {
                return CanNavigateBeyondEnd || currentPage < _pages.Count - 1;
            }
        }

        public bool CanNavigateBackward
        {
            get
            {
                return CanNavigateBeyondBeginning || currentPage > 0;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler BeginningReached;
        public event EventHandler EndReached;

        public WizardPageManager()
        {
            CanNavigateBeyondBeginning = false;
            CanNavigateBeyondEnd = false;
        }

        public void NavigateForward()
        {
            if (currentPage < _pages.Count - 1)
                CurrentPage = _pages[++currentPage];
            else if (EndReached != null)
                EndReached(this, new EventArgs());

            NotfiyNavigateForwardBackwardMightHaveChanged();
        }

        public void NavigateBackward()
        {
            if (currentPage > 0)
                CurrentPage = _pages[--currentPage];
            else if (BeginningReached != null)
                BeginningReached(this, new EventArgs());

            NotfiyNavigateForwardBackwardMightHaveChanged();
        }

        protected void NotifyPropertyChanged(string parameterName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(parameterName));
        }

        internal void Add(WizardPageViewModelBase view)
        {
            _pages.Add(view);
            if (_pages.Count == 1)
                CurrentPage = _pages[0];
        }

        private void NotfiyNavigateForwardBackwardMightHaveChanged()
        {
            NotifyPropertyChanged("CanNavigateBackward");
            NotifyPropertyChanged("CanNavigateForward");
        }
    }
}
