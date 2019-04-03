// Author:  Jordan Travaux
// Date:    Feb 26, 2019
// Purpose: Class for relaying commands from the GUI

using System;
using System.Windows.Input;

namespace MosaicBoard.Helpers
{
    public class RelayCommand : ICommand
    {
        private readonly Action Action;
        public event EventHandler CanExecuteChanged {add { } remove { } }

        public RelayCommand(Action action) {
            Action = action;
        }

        public void Execute(object parameter) {
            Action();
        }

        public bool CanExecute(object parameter) {
            return true;
        }
    }
}
