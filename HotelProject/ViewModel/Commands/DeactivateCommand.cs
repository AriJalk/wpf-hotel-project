using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands
{
    class DeactivateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private ViewModelBase _vm;

        public DeactivateCommand(ViewModelBase vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter != null)
                return true;
            return false;
        }

        public void Execute(object parameter)
        {
            _vm.DeactivateSelectedItem();
        }
    }
}
