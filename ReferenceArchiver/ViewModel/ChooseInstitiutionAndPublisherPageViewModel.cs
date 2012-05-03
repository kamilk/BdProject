using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using ReferenceArchiver.Model;

namespace ReferenceArchiver.ViewModel
{
    class ChooseInstitiutionAndPublisherPageViewModel : WizardPageViewModelBase
    {
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

        public ChooseInstitiutionAndPublisherPageViewModel(WizardViewModel parent, List<Institution> institutions, List<Publisher> publishers)
            : base(parent)
        {
            Institutions = new CollectionViewSource { Source = institutions }.View;
            Institutions.Filter = new Predicate<object>(FilterInstitutions);
            Institutions.MoveCurrentTo(null);
            Institutions.CurrentChanged += new EventHandler(Institutions_CurrentChanged);
            Publishers = new CollectionViewSource { Source = publishers }.View;
            Publishers.Filter = new Predicate<object>(FilterPublishers);
        }

        void Institutions_CurrentChanged(object sender, EventArgs e)
        {
            Publishers.Refresh();
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
