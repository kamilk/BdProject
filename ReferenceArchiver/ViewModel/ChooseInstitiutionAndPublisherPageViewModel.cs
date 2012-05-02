using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReferenceArchiver.Model;
using System.ComponentModel;
using System.Windows.Data;

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
                Institutions.Filter = new Predicate<object>(FilterInstitutions); //TODO find something better than this workaround
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
                Publishers.Filter = new Predicate<object>(FilterPublishers);
                Publishers.Refresh();
            }
        }

        public ChooseInstitiutionAndPublisherPageViewModel(WizardViewModel parent)
            : base(parent)
        {
            Institutions = CollectionViewSource.GetDefaultView(CentralRepository.Instance.GetInstitutions());
            Institutions.Filter = new Predicate<object>(FilterInstitutions);
            Institutions.MoveCurrentTo(null);
            Institutions.CurrentChanged += new EventHandler(Institutions_CurrentChanged);
            Publishers = CollectionViewSource.GetDefaultView(CentralRepository.Instance.GetPublishers());
            Publishers.Filter = new Predicate<object>(FilterPublishers);
        }

        void Institutions_CurrentChanged(object sender, EventArgs e)
        {
            Publishers.Filter = new Predicate<object>(FilterPublishers);
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
