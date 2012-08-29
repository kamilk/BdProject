using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using ReferenceArchiver.Model;
using System.Windows.Input;
using ReferenceArchiver.ViewModel.Helpers;

namespace ReferenceArchiver.ViewModel
{
    class ChooseInstitiutionAndPublisherPageViewModel : WizardPageViewModelBase
    {
        private bool _selectedInstitutionIsNull;

        public override string Title
        {
            get { return "Wybierz instytucję i wydawnictwo"; }
        }

        private SearchableCollectionViewWrapper<Institution> _institutions;
        public ICollectionView Institutions
        {
            get
            {
                return _institutions.CollectionView;
            }
        }

        private SearchableCollectionViewWrapper<Publisher> _publishers;
        public ICollectionView Publishers
        {
            get
            {
                return _publishers.CollectionView;
            }
        }

        public string InstitutionFilteringString
        {
            get
            {
                return _institutions.SearchString;
            }
            set
            {
                _institutions.SearchString = value;
                NotifyPropertyChanged("InstitutionFilteringString");
                Institutions.Refresh();
            }
        }

        public string PublisherFilteringString
        {
            get
            {
                return _publishers.SearchString;
            }
            set
            {
                _publishers.SearchString = value;
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

        private DelegateCommand _deselectInstitutionCommand;
        public ICommand DeselectInstitutionCommand
        {
            get
            {
                if (_deselectInstitutionCommand == null)
                {
                    _deselectInstitutionCommand = new DelegateCommand(
                    (param) =>
                    {
                        Publishers.MoveCurrentTo(null);
                        Institutions.MoveCurrentTo(null);
                        _deselectInstitutionCommand.RaiseCanExecuteChanged();
                    },
                    (param) =>
                    {
                        return Institutions.CurrentItem != null;
                    });
                }
                return _deselectInstitutionCommand;
            }
        }

        public ChooseInstitiutionAndPublisherPageViewModel(WizardViewModel parent, List<Institution> institutions, List<Publisher> publishers)
            : base(parent)
        {
            _institutions = new SearchableCollectionViewWrapper<Institution>(
                new CollectionViewSource { Source = institutions }.View, 
                institution => institution.Name);
            _selectedInstitutionIsNull = true;
            Institutions.MoveCurrentTo(null);
            Institutions.CurrentChanged += new EventHandler(Institutions_CurrentChanged);

            _publishers = new SearchableCollectionViewWrapper<Publisher>(
                new CollectionViewSource { Source = publishers }.View,
                publisher => publisher.Title,
                publisher =>
                {
                    //filter out publishers not belonging to the currently selected institution.
                    //If no institution is selected, show all publishers.
                    var institution = Institutions.CurrentItem as Institution;
                    return institution == null || publisher.InstitutionId == institution.Id;
                });
            Publishers.CurrentChanged += new EventHandler(Publishers_CurrentChanged);
            Publishers.MoveCurrentTo(null);
        }

        public override bool IsDataComplete
        {
            get
            {
                return SelectedInstitution != null && SelectedPublisher != null;
            }
        }

        void Institutions_CurrentChanged(object sender, EventArgs e)
        {
            if (!_selectedInstitutionIsNull || SelectedInstitution != null)
                Publishers.Refresh();
            _selectedInstitutionIsNull = SelectedInstitution == null;
            if (!_selectedInstitutionIsNull)
                Publishers.MoveCurrentToFirst();
            _deselectInstitutionCommand.RaiseCanExecuteChanged();
            NotifyIsDataCompleteChanged();
        }

        void Publishers_CurrentChanged(object sender, EventArgs e)
        {
            var publisher = SelectedPublisher;
            if (publisher == null)
                return;

            var institution = SelectedInstitution;
            if (institution == null || institution.Id != publisher.InstitutionId)
            {
                SelectInstitutionOwningPublisher(publisher);
            }

            NotifyIsDataCompleteChanged();
        }

        private void SelectInstitutionOwningPublisher(Publisher publisher)
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
}
