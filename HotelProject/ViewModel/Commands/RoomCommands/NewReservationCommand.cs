using HotelProject.Model.DbClasses;
using HotelProject.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands.RoomCommands
{
    public class NewReservationCommand : ICommand
    {
        private DisplayRoomFullVM _vm;

        public event EventHandler CanExecuteChanged;


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Debug.WriteLine("RESERVATION NEW");
            _vm.OpenCreditView();
        }

        public NewReservationCommand(DisplayRoomFullVM vm)
        {
            _vm = vm;
        }
    }
}
