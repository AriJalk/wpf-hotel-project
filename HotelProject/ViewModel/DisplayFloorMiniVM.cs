using HotelProject.Model.DbClasses;
using HotelProject.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
//TODO: DELETE
namespace HotelProject.ViewModel
{
    class DisplayFloorMiniVM : ViewModelBase
    {
        private Floor _floor;

        public Floor Floor
        {
            get { return _floor; }
            set
            {
                SetProperty(ref _floor, value);
            }
        }

        public DisplayFloorMiniVM(Floor floor)
        {
            Floor = floor;
        }
    }
}
