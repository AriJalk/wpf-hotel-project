using HotelProject.Model.DbClasses;
using HotelProject.ViewModel.Commands.FloorRoomCommand;
using HotelProject.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelProject.ViewModel
{
    class EditFloorRoomViewVM : ViewModelBase, IPageViewModel
    {
        public string Name => "Floor/Room Edit";

        private bool _showbutton;
        public bool ShowButton
        {
            get { return _showbutton; }
            set
            {
                _showbutton = value;
                OnPropertyChanged("ShowButton");
            }
        }

        private ObservableCollection<Floor> _floorcollection;

        public ObservableCollection<Floor> FloorCollection
        {
            get { return _floorcollection; }
            set
            { 
                _floorcollection = value;
                OnPropertyChanged("FloorCollection");
            }
        }

        private ObservableCollection<Room> _roomcollection;

        public ObservableCollection<Room> RoomCollection
        {
            get { return _roomcollection; }
            set
            {
                _roomcollection = value;
                OnPropertyChanged("RoomCollection");
            }
        }

        private ObservableCollection<RoomType> _roomtypecollection;

        public ObservableCollection<RoomType> RoomTypeCollection
        {
            get { return _roomtypecollection; }
            set 
            { 
                _roomtypecollection = value;
                OnPropertyChanged("RoomTypeCollection");
            }
        }


        private NewFloorCommand _newFloorCommand;

        public NewFloorCommand NewFloorCommand
        {
            get { return _newFloorCommand; }
            set 
            {
                _newFloorCommand = value;
                OnPropertyChanged("NewFloorCommand");
            }
        }

        private NewRoomCommand _newRoomCommand;

        public NewRoomCommand NewRoomCommand
        {
            get { return _newRoomCommand; }
            set 
            {
                _newRoomCommand = value;
                OnPropertyChanged("NewRoomCommand");
            }
        }

        private string _newFloorNumber;

        public string NewFloorNumber
        {
            get { return _newFloorNumber; }
            set 
            {
                _newFloorNumber = value;
                OnPropertyChanged("NewFloorNumber");
            }
        }
        private string _newRoomNumber;

        public string NewRoomNumber
        {
            get { return _newRoomNumber; }
            set
            {
                _newRoomNumber = value;
                OnPropertyChanged("NewRoomNumber");
            }
        }


        private string _newRoomFloorNumber;

        public string NewRoomFloorNumber
        {
            get { return _newRoomFloorNumber; }
            set
            {
                _newRoomFloorNumber = value;
                OnPropertyChanged("NewRoomFloorNumber");
            }
        }

        private string _newRoomRowNumber;

        public string NewRoomRowNumber
        {
            get { return _newRoomRowNumber; }
            set
            {
                _newRoomRowNumber = value;
                OnPropertyChanged("NewRoomRowNumber");
            }
        }

        private RoomType _selectedRoomType;

        public RoomType SelectedRoomType
        {
            get { return _selectedRoomType; }
            set 
            {
                _selectedRoomType = value;
                OnPropertyChanged("SelectedRoomType");
            }
        }

        private string _capacity;

        public string Capacity
        {
            get { return _capacity; }
            set { _capacity = value; }
        }



        private Room _backupRoom;

        private Room _selectedRoom;

        public Room SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                bool valid = true;
                //Save or revert object
                if (_selectedRoom != null)
                {
                    if (_selectedRoom.ValidateData())
                    {
                        SqlDatabaseHelper.Insert(_selectedRoom);
                        _selectedRoom.IsInDb = true;
                    }
                    else
                    {
                        if (_selectedRoom.IsInDb)
                        {
                            MessageBox.Show("Info not correct, reverting");
                            _selectedRoom = _backupRoom;
                            valid = false;
                            Refresh();
                        }
                        else
                        {
                            _selectedRoom.IdCount--;
                            _selectedRoom = null;
                            valid = false;
                            Refresh();
                        }
                    }
                }
                if (value != null && valid)
                {
                    _selectedRoom = value;
                    _backupRoom = new Room(value);
                    AppVm.Globals.SelectedRoom = _selectedRoom;
                    AppVm.Globals.SelectedFloor = _selectedFloor;
                }
                OnPropertyChanged("SelectedRoom");
            }
        }

        private Floor _backupFloor;

        private Floor _selectedFloor;

        public Floor SelectedFloor
        {
            get { return _selectedFloor; }
            set
            {
                bool valid = true;
                //Save or revert object
                if (_selectedFloor != null)
                {
                    if (_selectedFloor.ValidateData())
                    {
                        SqlDatabaseHelper.Insert(_selectedFloor);
                        _selectedFloor.IsInDb = true;
                    }
                    else
                    {
                        if (_selectedFloor.IsInDb)
                        {
                            MessageBox.Show("Info not correct, reverting");
                            _selectedFloor = _backupFloor;
                            valid = false;
                            Refresh();
                        }
                        else
                        {
                            _selectedFloor.IdCount--;
                            _selectedFloor = null;
                            valid = false;
                            Refresh();
                        }
                    }
                }
                if (value != null && valid)
                {
                    _selectedFloor = value;
                    _backupFloor = new Floor(value);
                }
                OnPropertyChanged("SelectedFloor");
            }
        }
        public EditFloorRoomViewVM(ApplicationViewModel vm)
        {
            AppVm = vm;
            NewFloorCommand = new NewFloorCommand(this);
            NewRoomCommand = new NewRoomCommand(this);
        }


        public void Dispose()
        {
            _floorcollection = null;
            _roomcollection = null;
        }


        public void Refresh()
        {
            List<Floor> floorList = SqlDatabaseHelper.Read<Floor>();
            List<Room> roomList = SqlDatabaseHelper.Read<Room>();
            SqlDatabaseHelper.JoinDiscreteByInnerOneWay(roomList,AppVm.Globals.RoomTypes);
            SqlDatabaseHelper.JoinLists(floorList, roomList);
            FloorCollection = new ObservableCollection<Floor>(floorList);
            RoomCollection = new ObservableCollection<Room>(roomList);
            RoomTypeCollection = new ObservableCollection<RoomType>(AppVm.Globals.RoomTypes);
            _selectedRoomType = null;
        }

        public void NewFloor()
        {
            int num;
            bool isNumber=int.TryParse(NewFloorNumber, out num);
            bool isValid = true;
            if (isNumber)
            {
                foreach (Floor floor in FloorCollection)
                {
                    if (floor.ElementNumber == num)
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            //Insert if valid
            if (isValid)
            {
                SqlDatabaseHelper.Insert(new Floor(num));
            }
            Refresh();
        }

        public void NewRoom()
        {
            int roomnum,floornum,rownum,capacitynum;
            bool isNumber = int.TryParse(NewRoomNumber, out roomnum);
            bool isFloorNumber = int.TryParse(NewRoomFloorNumber, out floornum);
            bool isRowNumber = int.TryParse(NewRoomRowNumber, out rownum);
            bool isCapacityValid = int.TryParse(Capacity, out capacitynum);
            bool isValid = true;
            Floor selectedFloor=null;
            if (isNumber&&isFloorNumber&&isRowNumber&&isCapacityValid)
            {
                foreach (Room room in RoomCollection)
                {
                    //Test if exists
                    if (room.ElementNumber == roomnum)
                    {
                        isValid = false;
                        break;
                    }
                }
                if(isValid)
                {
                    isValid = false;
                    foreach(Floor floor in FloorCollection)
                    {
                        if (floor.ElementNumber == floornum)
                        {
                            isValid = true;
                            selectedFloor = floor;
                            break;
                        }
                    }
                }
            }
            //Insert if valid
            if (isValid)
            {
                SqlDatabaseHelper.Insert(new Room(roomnum, SelectedRoomType, selectedFloor, capacitynum, rownum));
            }
            Refresh();
        }
    }
}
