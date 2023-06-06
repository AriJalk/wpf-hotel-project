using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;

namespace HotelProject.ViewModel.Containers
{
    /// <summary>
    /// Container class holding a row of Room ViewModels for binding
    /// </summary>
    public class RoomRowVmContainer : ViewModelBase
    {
        private FloorsViewVM _parentvm;

        public FloorsViewVM ParentVm
        {
            get { return _parentvm; }
            set
            {
                _parentvm = value;
                OnPropertyChanged("ParentVm");
            }
        }

        private ObservableCollection<DisplayRoomMiniVM> _vmcollection;

        private ObservableCollection<DisplayRoomMiniVM> VmCollection
        {
            get { return _vmcollection; }
            set 
            { 
                _vmcollection = value;
                OnPropertyChanged("VmCollection");
            }
        }

        private ICollectionView _collectionview;

        public ICollectionView CollectionView
        {
            get 
            { 
                return _collectionview; 
            }
            set 
            {
                _collectionview = value;
                OnPropertyChanged("CollectionView");
            }
        }

        public enum FilterTypes
        {
            Available,
            Unavailable
        }


        public RoomRowVmContainer()
        {
            VmCollection = new ObservableCollection<DisplayRoomMiniVM>();
            CollectionView = CollectionViewSource.GetDefaultView(_vmcollection);
            CollectionView.Filter = DisplayActive;
            CollectionView.SortDescriptions.Add
                ((new SortDescription(nameof(DisplayRoomMiniVM.Room.ElementNumber), ListSortDirection.Ascending)));
        }

        public void AddToCollection(DisplayRoomMiniVM vm, FloorsViewVM parentvm)
        {
            VmCollection.Add(vm);
            OnPropertyChanged("VmCollection");
            ParentVm = parentvm;
        }

        public void SetFilter(bool showUnavailable)
        {
            if (showUnavailable == true)
                CollectionView.Filter = DisplayNotActive;
            else
                CollectionView.Filter = DisplayActive;
            OnPropertyChanged("CollectionView");

        }

        public void ClearCollection()
        {
            VmCollection.Clear();
            OnPropertyChanged("VmCollection");
        }

        private bool DisplayActive(object obj)
        {
            DisplayRoomMiniVM vm = obj as DisplayRoomMiniVM;
            if (vm != null)
            {
                Debug.WriteLine("Is Active: " + vm.Room.IsActive);
                return vm.Room.IsActive;
            }
            return false;
        }
        private bool DisplayNotActive(object obj)
        {
            DisplayRoomMiniVM vm = obj as DisplayRoomMiniVM;
            if (vm != null)
            {
                Debug.WriteLine("Is Active: " + vm.Room.IsActive);
                return true;
            }
            return false;
        }
    }
}
