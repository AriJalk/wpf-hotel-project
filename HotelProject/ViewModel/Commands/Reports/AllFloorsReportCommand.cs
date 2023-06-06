using System;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands.Reports
{
    class AllFloorsReportCommand : ICommand
    {
        private ReportsViewVM _vm;

        public event EventHandler CanExecuteChanged;

        public AllFloorsReportCommand(ReportsViewVM vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _vm.AllFloorsReport();
        }
    }
}
