using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ReferenceArchiver.ViewModel.Helpers
{
    /// <summary>
    /// Class responsible for navigation between pages (steps) of the main window wizard.
    /// </summary>
    internal class WizardPageManager : INotifyPropertyChanged
    {
        List<WizardPageViewModelBase> _pages = new List<WizardPageViewModelBase>();

        int currentPage = 0;

        WizardPageViewModelBase _currentPage;

        /// <summary>
        /// Gets or sets the page which should be currently displayed.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
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

        /// <summary>
        /// Gets or sets a value indicating whether the user CanNavigateBackward should return true
        /// when currently displayed is the first page of the wizard.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can navigate beyond beginning; otherwise, <c>false</c>.
        /// </value>
        public bool AllowNavigationBeyondBeginning { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user CanNavigateForward should return true
        /// when currently displayed is the last page of the wizard.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can navigate beyond end; otherwise, <c>false</c>.
        /// </value>
        public bool AllowNavigationBeyondEnd { get; set; }

        /// <summary>
        /// Gets a value indicating whether navigating forward is currently possible.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can navigate forward; otherwise, <c>false</c>.
        /// </value>
        public bool CanNavigateForward
        {
            get
            {
                return (AllowNavigationBeyondEnd || currentPage < _pages.Count - 1) && CurrentPage.IsDataComplete;
            }
        }

        /// <summary>
        /// Gets a value indicating whether navigating backward is currently possible.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can navigate backward; otherwise, <c>false</c>.
        /// </value>
        public bool CanNavigateBackward
        {
            get
            {
                return AllowNavigationBeyondBeginning || currentPage > 0;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when attemted to navigate beyond the beginning of the wizard.
        /// </summary>
        public event EventHandler BeginningReached;

        /// <summary>
        /// Occurs when attemted to navigate beyond the end of the wizard.
        /// </summary>
        public event EventHandler EndReached;

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardPageManager" /> class.
        /// </summary>
        public WizardPageManager()
        {
            AllowNavigationBeyondBeginning = false;
            AllowNavigationBeyondEnd = false;
        }

        /// <summary>
        /// Navigates forward. 
        /// </summary>
        /// <remarks>
        /// This method should either update CurrentPage or raise EndReached event.
        /// </remarks>
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

        /// <summary>
        /// Navigates backward. 
        /// </summary>
        /// <remarks>
        /// This method should either update CurrentPage or raise BeginningReached event.
        /// </remarks>
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

        /// <summary>
        /// Forces PropertyChanged event to be raised on CanNavigateForward and CanNavigateBackward.
        /// </summary>
        public void RefreshCanNavigate()
        {
            NotfiyNavigateForwardBackwardMightHaveChanged();
        }

        /// <summary>
        /// Adds the specified page at the end of the list of pages managed by this WizardPageManager.
        /// </summary>
        /// <param name="view">The view.</param>
        internal void Add(WizardPageViewModelBase view)
        {
            _pages.Add(view);
            if (_pages.Count == 1)
                CurrentPage = _pages[0];
        }

        private void NotifyPropertyChanged(string parameterName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(parameterName));
        }

        private void NotfiyNavigateForwardBackwardMightHaveChanged()
        {
            NotifyPropertyChanged("CanNavigateBackward");
            NotifyPropertyChanged("CanNavigateForward");
        }
    }
}
