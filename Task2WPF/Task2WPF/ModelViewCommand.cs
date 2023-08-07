using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Task2WPF
{
    /// <summary>
    /// Class that provides functions to execute commands in WPF and attach some actions to them
    /// </summary>
    internal class ModelViewCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        private Predicate<object> _canExecute;
        private Action<object> _execute;
        public ModelViewCommand(Action<object> command, Predicate<object> canExecute = null)
        {
            _execute = command;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            this._execute(parameter);
        }
    }
}
