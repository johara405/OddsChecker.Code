using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OddsChecker
{
    class ProcessCommand : ICommand
    {

        private OddCheckerViewModel _viewmodel;

        public ProcessCommand(OddCheckerViewModel vm)
        {
            _viewmodel = vm;
        }

        public void Execute(object parameter)
        {
            _viewmodel.StartProcess();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

       
    }
}

