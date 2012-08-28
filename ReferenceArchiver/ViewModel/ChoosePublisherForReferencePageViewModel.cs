using System.Collections.Generic;
using ReferenceArchiver.Model;
using System.Windows.Input;

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

        DelegateCommand _switchToStandardPublisherCommand;
        public ICommand SwitchToStandardPublisherCommand
        {
            get
            {
                if (_switchToStandardPublisherCommand == null)
                    _switchToStandardPublisherCommand = new DelegateCommand(SwitchToStandardPublisher);
                return _switchToStandardPublisherCommand;
            }
        }

        DelegateCommand _switchToExternalPublisherCommand;
        public ICommand SwitchToExternalPublisherCommand
        {
            get
            {
                if (_switchToExternalPublisherCommand == null)
                    _switchToExternalPublisherCommand = new DelegateCommand(SwitchToStandardPublisher);
                return _switchToExternalPublisherCommand;
            }
        }

        public ChoosePublisherForReferencePageViewModel(WizardViewModel parent, List<Institution> institutions, List<Publisher> publishers)
            : base(parent)
        {
            _standardViewModel = new ChooseInstitiutionAndPublisherPageViewModel(parent, institutions, publishers);
            _externalViewModel = new ChooseExternalPublisherViewModel(parent);
            SelectedPublisherType = _standardViewModel;
        }

        private void SwitchToStandardPublisher()
        {
            SelectedPublisherType = _standardViewModel;
        }

        private void SwitchToExternalPublisher()
        {
            SelectedPublisherType = _externalViewModel;
        }
    }
}
