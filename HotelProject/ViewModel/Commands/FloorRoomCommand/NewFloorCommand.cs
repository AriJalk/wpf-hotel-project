using System;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands.FloorRoomCommand
{
    class NewFloorCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private EditFloorRoomViewVM _vm;

        public NewFloorCommand(EditFloorRoomViewVM vm)
        {
            _vm = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _vm.NewFloor();
        }
    }
}
