using System.Collections.Generic;
using ReferenceArchiver.Model;

namespace ReferenceArchiver.ViewModel
{
    class ChoosePublisherForReferencePageViewModel : WizardPageViewModelBase
    {
        ChooseInstitiutionAndPublisherPageViewModel _standardViewModel;
        ChooseExternalPublisherViewModel _externalViewModel;

        public override string Title
        {
            get { return "Wybierz wydawnictwo artykułu wymienionego w przypisie"; }
        }

        private WizardControlViewModelBase _selectedPublisherType;
        public WizardControlViewModelBase SelectedPublisherType
        {
            get
            {
                return _selectedPublisherType;
            }
            set
            {
                _selectedPublisherType = value;
                NotifyPropertyChanged("SelectedPublisherType");
            }
        }

        public ChoosePublisherForReferencePageViewModel(WizardViewModel parent, List<Institution> institutions, List<Publisher> publishers)
            : base(parent)
        {
            _standardViewModel = new ChooseInstitiutionAndPublisherPageViewModel(parent, institutions, publishers);
            _externalViewModel = new ChooseExternalPublisherViewModel(parent);
            SelectedPublisherType = _standardViewModel;
        }

        public void SwitchToStandardPublisher()
        {
            SelectedPublisherType = _standardViewModel;
        }

        public void SwitchToExternalPublisher()
        {
            SelectedPublisherType = _externalViewModel;
        }
    }
}
