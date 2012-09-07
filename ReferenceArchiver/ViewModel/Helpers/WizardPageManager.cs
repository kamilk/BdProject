using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ReferenceArchiver.ViewModel.Helpers
{
    internal class WizardPageManager : INotifyPropertyChanged
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
                return (CanNavigateBeyondEnd || currentPage < _pages.Count - 1) && CurrentPage.IsDataComplete;
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
            var oldPage = CurrentPage;

            if (currentPage < _pages.Count - 1)
                CurrentPage = _pages[++currentPage];
            else if (EndReached != null)
                EndReached(this, new EventArgs());

            oldPage.OnNavigatedFromForward();
            CurrentPage.OnNavigatedTo(NavigationDirection.Forward);

            NotfiyNavigateForwardBackwardMightHaveChanged();
        }

        public void NavigateBackward()
        {
            var oldPage = CurrentPage;

            if (currentPage > 0)
                CurrentPage = _pages[--currentPage];
            else if (BeginningReached != null)
                BeginningReached(this, new EventArgs());

            oldPage.OnNavigatedFromBackward();
            CurrentPage.OnNavigatedTo(NavigationDirection.Backward);

            NotfiyNavigateForwardBackwardMightHaveChanged();
        }

        public void RefreshCanNavigate()
        {
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
