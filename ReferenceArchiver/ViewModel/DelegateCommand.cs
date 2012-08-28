using System;
using System.Windows.Input;

//from http://www.wpftutorial.net/delegatecommand.html
namespace ReferenceArchiver.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _executeWithParameters;
        private readonly Action _executeNoParameters;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _executeWithParameters = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action execute, Predicate<object> canExecute = null)
        {
            _executeNoParameters = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (_executeWithParameters != null)
                _executeWithParameters(parameter);
            else
                _executeNoParameters();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
