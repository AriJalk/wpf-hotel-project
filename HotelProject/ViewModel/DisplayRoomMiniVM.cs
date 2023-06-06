using HotelProject.Model.DbClasses;
using System;

namespace HotelProject.ViewModel
{
    public class DisplayRoomMiniVM : ViewModelBase
    {
        private Room _room;

        public Room Room
        {
            get 
            { 
                return _room; 
            }
            set
            {
                _room = value;
                ElementNumber = _room.ElementNumber;
                RoomTypeId = _room.RoomTypeId;
                OnPropertyChanged("Room");

            }
        }

        private int _elementnumber;
        //Used for sorting
        public int ElementNumber
        {
            get
            { 
                return _elementnumber;
            }
            set
            { 
                _elementnumber = value;
                OnPropertyChanged("ElementNumber");
            }
        }

        private int _roomtypeid;

        public int RoomTypeId
        {
            get
            { 
                return _roomtypeid;
            }
            set 
            { 
                _roomtypeid = value;
                OnPropertyChanged("RoomTypeId");
            }
        }

        private FloorsViewVM _parentvm;

        public FloorsViewVM ParentVm
        {
            get { return _parentvm; }
            set { _parentvm = value; }
        }


        private bool _isroomavailable;

        public bool IsRoomAvailable
        {
            get 
            {
                return _isroomavailable;
            }
            set 
            {
                _isroomavailable = value;
                OnPropertyChanged("IsRoomAvailable");
            }
        }


        public DisplayRoomMiniVM(Room room, FloorsViewVM vm)
        {
            ParentVm = vm;
            Room = room;
            ElementNumber = Room.ElementNumber;
        }

        public DisplayRoomMiniVM()
        {
            //Room = new Room();
        }

        public void RefreshRoomVM(DateTime startTime,DateTime endTime, int numpeople)
        {
            ElementNumber = Room.ElementNumber;
            IsRoomAvailable =
                    Room.IsRoomAvailable(startTime, endTime, numpeople)
                    && startTime >= DateTime.Now
                    && endTime >= DateTime.Now;
            RoomTypeId = Room.RoomTypeId;
        }
    }
}
