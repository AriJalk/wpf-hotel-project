using System;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands
{
    public class UpTextBlockCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public ViewModelBase VM { get; set; }

        public UpTextBlockCommand(ViewModelBase vm)
        {
            VM = vm;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.IncrementProperty();
        }
    }
}
