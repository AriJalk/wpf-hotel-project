using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands.DbManagement
{
    class BackupDbCommand : ICommand
    {
        private DbManagementViewVM Vm;

        public event EventHandler CanExecuteChanged;

        public BackupDbCommand(DbManagementViewVM vm)
        {
            this.Vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Vm.BackupDB();
        }
    }
}
