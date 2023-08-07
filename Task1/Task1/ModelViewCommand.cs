using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Task1
{
    internal class ModelViewCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        private Predicate<object> _canExecute;
        private Action _execute;
        public ModelViewCommand(Action command, Predicate<object> canExecute = null)
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
            this._execute();
        }
    }
}
