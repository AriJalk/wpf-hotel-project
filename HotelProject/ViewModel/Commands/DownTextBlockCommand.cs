using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelProject.ViewModel.Commands
{
    public class DownTextBlockCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public ViewModelBase VM { get; set; }

        public DownTextBlockCommand(ViewModelBase vm)
        {
            VM = vm;
        }


        public bool CanExecute(object parameter)
        {
            if (parameter != null)
            {
                int num = (int)parameter;
                if (num == 1)
                    return false;

            }
            return true;
        }

        public void Execute(object parameter)
        {
            VM.DecrementProperty();
        }
    }
}
