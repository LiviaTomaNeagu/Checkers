﻿using System;
using System.Windows.Input;

namespace CheckersImpl.Commands
{
    internal class SaveGameCommand : ICommand
    {
        private readonly Action _execute;

        public SaveGameCommand(Action execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public bool CanExecute(object parameter)
        {
            return true; // Always enabled
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
