using HotelProject.Model.DbClasses;
using System;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands
{
    public class LoginCommand : ICommand
    {
        private HomeViewVM _vm;

        public event EventHandler CanExecuteChanged;

        public LoginCommand(HomeViewVM vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            User user = parameter as User;
            if (user != null)
            {
                return true;
            }
            return true;
        }

        public void Execute(object parameter)
        {
            _vm.Login();
        }
    }
}
