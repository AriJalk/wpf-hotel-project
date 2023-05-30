using HotelProject.Model.DbClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands
{
    public class LogoutCommand : ICommand
    {
        private HomeViewVM _vm;

        public event EventHandler CanExecuteChanged;

        public LogoutCommand(HomeViewVM vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _vm.Logout();
        }
    }
}
