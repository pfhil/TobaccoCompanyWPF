﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TobaccoCompanyWPF.ViewModels.MVVM
{
    public class Command : ICommand
    {
        private readonly Action<object?> _action;
        private readonly Func<object?, bool>? _canExecute;

        public Command(Action<object?> action, Func<object?, bool>? canExecute = null) => (_action, _canExecute) = (action, canExecute);

        public void Execute(object? parameter) => _action(parameter);

        public bool CanExecute(object? parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
