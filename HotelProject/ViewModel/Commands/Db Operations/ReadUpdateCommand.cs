using HotelProject.Model.BaseClasses;
using HotelProject.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands.Db_Operations
{
    class ReadUpdateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //TODO
            return true;
        }

        public void Execute(object parameter)
        {
            SqlDatabaseHelper.Insert(parameter as HotelDbElementBase);
        }
    }
}
