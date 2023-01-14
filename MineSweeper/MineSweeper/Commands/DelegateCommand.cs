using System;
using System.Windows.Input;

namespace MineSweeper.Commands
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private Func<object?, bool> CanExecuteDelegate { get; init; }
        private Action<object?> ExecuteCommandDelegate { get; init; }

        public DelegateCommand(Action<object?> execCmd, Func<object?, bool> canExec)
        {
            CanExecuteDelegate = canExec;
            ExecuteCommandDelegate = execCmd;
        }

        public DelegateCommand(Action<object?> execCmd)
        {
            CanExecuteDelegate = (_) => true;
            ExecuteCommandDelegate = execCmd;
        }

        public bool CanExecute(object? parameter) => CanExecuteDelegate(parameter);

        public void Execute(object? parameter) => ExecuteCommandDelegate(parameter);
    }
}
