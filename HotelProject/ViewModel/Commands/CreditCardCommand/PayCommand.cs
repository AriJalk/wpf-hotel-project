using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands.CreditCardCommand
{
    public class PayCommand : ICommand
    {
        private CreditCardViewVM _vm;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _vm.Pay();
        }

        public PayCommand(CreditCardViewVM vm)
        {
            _vm = vm;
        }
    }
}
