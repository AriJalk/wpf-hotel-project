using System;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands.Reports
{
    class AllCustomersReportCommand : ICommand
    {
        private ReportsViewVM _vm;

        public event EventHandler CanExecuteChanged;

        public AllCustomersReportCommand(ReportsViewVM vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _vm.AllCustomersReport();
        }
    }
}
