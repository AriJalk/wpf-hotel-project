using System;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands.Reports
{
    class AllRoomsReportCommand : ICommand
    {
        private ReportsViewVM _vm;

        public event EventHandler CanExecuteChanged;

        public AllRoomsReportCommand(ReportsViewVM vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _vm.AllRoomsReport();
        }
    }
}
