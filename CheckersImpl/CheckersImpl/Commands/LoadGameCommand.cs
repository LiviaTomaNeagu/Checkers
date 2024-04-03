using System;
using System.Windows.Input;

namespace CheckersImpl.Commands
{
    internal class LoadGameCommand : ICommand
    {
        private readonly Action _execute;

        public LoadGameCommand(Action execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public bool CanExecute(object parameter)
        {
            // Here, you can optionally add logic to determine 
            // whether the command is currently valid to execute.
            // For simplicity, this returns true to indicate it's always executable.
            return true;
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
