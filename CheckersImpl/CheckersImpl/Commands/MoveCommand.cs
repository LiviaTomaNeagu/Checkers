using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheckersImpl.Commands
{
    public class MoveCommand : ICommand
    {
        // Action to be executed when the command is invoked
        private Action _execute;

        // Function to determine if the command can be executed
        private Func<bool> _canExecute;

        // Event that is raised when changes occur that affect whether or not the command should execute
        public event EventHandler CanExecuteChanged;

        // Constructor
        public MoveCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // Method to determine if the command can execute in its current state
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? false;
        }

        // Method to execute the commands
        public void Execute(object parameter)
        {
            _execute?.Invoke();
        }

        // Method to raise the CanExecuteChanged event
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
