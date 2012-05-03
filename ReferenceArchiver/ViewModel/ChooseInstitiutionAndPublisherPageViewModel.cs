using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using ReferenceArchiver.Model;
using System.Windows.Input;

namespace ReferenceArchiver.ViewModel
{
    class ChooseInstitiutionAndPublisherPageViewModel : WizardPageViewModelBase
    {
        private bool _selectedInstitutionIsNull;

        public override string Title
        {
            get { return "Wybierz instytucję i wydawnictwo"; }
        }

        private ICollectionView _institutions;
        public ICollectionView Institutions
        {
            get
            {
                return _institutions;
            }
            set
            {
                _institutions = value;
                NotifyPropertyChanged("Institutions");
            }
        }

        private ICollectionView _publishers;
        public ICollectionView Publishers
        {
            get
            {
                return _publishers;
            }
            set
            {
                _publishers = value;
                NotifyPropertyChanged("Publishers");
            }
        }

        private string _institutionFilteringString;
        public string InstitutionFilteringString
        {
            get
            {
                return _institutionFilteringString;
            }
            set
            {
                _institutionFilteringString = value;
                NotifyPropertyChanged("InstitutionFilteringString");
                Institutions.Refresh();
            }
        }

        private string _publisherFilteringString;
        public string PublisherFilteringString
        {
            get
            {
                return _publisherFilteringString;
            }
            set
            {
                _publisherFilteringString = value;
                NotifyPropertyChanged("PublisherFilteringString");
                Publishers.Refresh();
            }
        }

        public Institution SelectedInstitution
        {
            get
            {
                return Institutions.CurrentItem as Institution;
            }
        }

        public Publisher SelectedPublisher
        {
            get
            {
                return Publishers.CurrentItem as Publisher;
            }
        }

        private DelegateCommand _deselectInstitution;
        public DelegateCommand DeselectInstitution
        {
            get
            {
                return _deselectInstitution;
            }
            private set
            {
                _deselectInstitution = value;
                NotifyPropertyChanged("DeselectInstitution");
            }
        }

        public ChooseInstitiutionAndPublisherPageViewModel(WizardViewModel parent, List<Institution> institutions, List<Publisher> publishers)
            : base(parent)
        {
            Institutions = new CollectionViewSource { Source = institutions }.View;
            Institutions.Filter = new Predicate<object>(FilterInstitutions);
            _selectedInstitutionIsNull = true;
            Institutions.MoveCurrentTo(null);
            Institutions.CurrentChanged += new EventHandler(Institutions_CurrentChanged);

            Publishers = new CollectionViewSource { Source = publishers }.View;
            Publishers.Filter = new Predicate<object>(FilterPublishers);
            Publishers.CurrentChanged += new EventHandler(Publishers_CurrentChanged);
            Publishers.MoveCurrentTo(null);

            this.DeselectInstitution = new DelegateCommand(
                (param) =>
                {
                    Publishers.MoveCurrentTo(null);
                    Institutions.MoveCurrentTo(null);
                    DeselectInstitution.RaiseCanExecuteChanged();
                },
                (param) =>
                {
                    return Institutions.CurrentItem != null;
                });
        }

        void Institutions_CurrentChanged(object sender, EventArgs e)
        {
            if (!(_selectedInstitutionIsNull && SelectedInstitution == null))
                Publishers.Refresh();
            _selectedInstitutionIsNull = SelectedInstitution == null;
            if (!_selectedInstitutionIsNull)
                Publishers.MoveCurrentToFirst();
            DeselectInstitution.RaiseCanExecuteChanged();
        }

        void Publishers_CurrentChanged(object sender, EventArgs e)
        {
            var publisher = SelectedPublisher;
            if (publisher == null)
                return;

            var institution = SelectedInstitution;
            if (institution == null || institution.Id != publisher.InstitutionId)
            {
                if (Institutions.MoveCurrentToFirst())
                {
                    while (!Institutions.IsCurrentAfterLast)
                    {
                        if (SelectedInstitution.Id == publisher.InstitutionId)
                            return;
                        Institutions.MoveCurrentToNext();
                    }
                }
                InstitutionFilteringString = "";
                if (Institutions.MoveCurrentToFirst())
                {
                    while (!Institutions.IsCurrentAfterLast)
                    {
                        if (SelectedInstitution.Id == publisher.InstitutionId)
                            return;
                        Institutions.MoveCurrentToNext();
                    }
                }
            }
        }


        private bool FilterInstitutions(object obj)
        {
            if (_institutionFilteringString == null || _institutionFilteringString.Length == 0)
                return true;

            var institution = obj as Institution;
            if (institution == null)
                return false;
            return institution.Name.StartsWith(_institutionFilteringString, StringComparison.CurrentCultureIgnoreCase);
        }

        private bool FilterPublishers(object obj)
        {
            var publisher = obj as Publisher;
            if (publisher == null)
                return false;

            var institution = Institutions.CurrentItem as Institution;

            bool nameMatch = PublisherFilteringString == null || PublisherFilteringString.Length == 0 || publisher.Title.StartsWith(PublisherFilteringString, StringComparison.CurrentCultureIgnoreCase);
            bool institutionMatch = institution == null || publisher.InstitutionId == institution.Id;
            return nameMatch && institutionMatch;
        }
    }
}
