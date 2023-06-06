using System;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands.Reports
{
    class ReservationsByCustomerCommand : ICommand
    {
        private ReportsViewVM _vm;

        public event EventHandler CanExecuteChanged;

        public ReservationsByCustomerCommand(ReportsViewVM vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _vm.ReservationsByCustomerReport();
        }
    }
}
