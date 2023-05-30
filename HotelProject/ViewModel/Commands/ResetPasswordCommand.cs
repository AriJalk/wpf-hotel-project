using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands
{
    class ResetPasswordCommand : ICommand
    {
        private UserViewVM _vm;

        public event EventHandler CanExecuteChanged;

        public ResetPasswordCommand(UserViewVM vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _vm.ResetPassword();
        }
    }
}
