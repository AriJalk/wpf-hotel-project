using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands
{
    class RefreshCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private IPageViewModel _vm;

        public RefreshCommand(IPageViewModel vm)
        {
            _vm = vm;
        }
        
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _vm.Refresh();
        }
    }
}
