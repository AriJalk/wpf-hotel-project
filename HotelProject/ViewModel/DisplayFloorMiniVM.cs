using HotelProject.Model.DbClasses;
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
