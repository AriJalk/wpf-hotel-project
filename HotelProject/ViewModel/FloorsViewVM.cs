using HotelProject.Model.DbClasses;
using HotelProject.Model.FileClasses;
using HotelProject.Model.Helpers;
using HotelProject.ViewModel.Commands;
using HotelProject.ViewModel.Containers;
using HotelProject.ViewModel.Helpers;
using HotelProject.ViewModel.Helpers.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HotelProject.ViewModel
{/// <summary>
 /// View model for the floor/room window
 /// Window is used to show a list of floors in the custom user control
 /// .
 /// Each floor contains a list of rooms which are assigned
 /// to their own ViewModels to be displayed in a custom user 
 /// interface DisplayRoomMini
 /// </summary>
    public class FloorsViewVM : ViewModelBase, IPageViewModel
    {
        public string Name => "Floors View";

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

        /* New merge section
         * 
         * 
         * 
         * 
         * */
        private HotelGlobalParameters _hotelglobalparameters;
        public HotelGlobalParameters HotelGlobalParameters
        {
            get { return _hotelglobalparameters; }
            set { _hotelglobalparameters = value; }
        }

        private Floor _selectedfloor;
        public Floor SelectedFloor
        {
            get { return _selectedfloor; }
            set
            {
                if (value != null)
                {
                    _selectedfloor = value;
                    SelectedRoomMiniVM = null;
                    IsRoomSelected = false;
                    int maxrow = 0;
                    RoomVMCollection.Clear();
                    foreach (var room in _selectedfloor.RoomList)
                    {
                        if (room.Row > maxrow)
                            maxrow = room.Row;
                        RoomVMCollection.Add(new DisplayRoomMiniVM(room, this));
                    }

                    VmRows.Clear();
                    for (int i = 0; i < maxrow; i++)
                    {
                        VmRows.Add(new RoomRowVmContainer());
                    }
                    foreach (Room room in _selectedfloor.RoomList)
                    {
                        VmRows[room.Row - 1].AddToCollection(new DisplayRoomMiniVM(room, this), this);
                        OnPropertyChanged("VmRows");
                    }
                    AppVm.Globals.SelectedFloor = SelectedFloor;
                    RefreshRoomVM();
                }
                OnPropertyChanged("SelectedFloor");
            }
        }

        private Room _selectedroom;

        public Room SelectedRoom
        {
            get { return _selectedroom; }
            set
            {
                if (value != null)
                {
                    _selectedroom = value;
                    Debug.WriteLine(_selectedroom.ElementNumber + " CHOSEN");
                    IsRoomSelected = true;
                    AppVm.Globals.SelectedRoom = SelectedRoom;
                }
                OnPropertyChanged("SelectedRoom");
            }
        }


        private bool _isroomselected;

        public bool IsRoomSelected
        {
            get { return _isroomselected; }
            set
            {
                _isroomselected = value;
                OnPropertyChanged("IsRoomSelected");
            }
        }

        private bool _isunavailablechecked;

        public bool IsUnavailableChecked
        {
            get { return _isunavailablechecked; }
            set
            {
                _isunavailablechecked = value;
                OnPropertyChanged("IsUnavailableChecked");
                RefreshUnavailableView();

            }
        }

        private DisplayRoomMiniVM _selectedroomminivm;
        /// <summary>
        /// The VM of the selected room, the selected room itself is set from the vm
        /// </summary>
        public DisplayRoomMiniVM SelectedRoomMiniVM
        {
            get
            {
                return _selectedroomminivm;
            }
            set
            {
                if (value != null)
                {
                    _selectedroomminivm = value;
                    SelectedRoom = _selectedroomminivm.Room;
                    SelectedRoomFullVM = new DisplayRoomFullVM(this);
                }
                OnPropertyChanged("SelectedRoomMiniVM");
            }
        }

        private int _eldsssdtlgejt;

        public int Something
        {
            get { return _eldsssdtlgejt; }
            set { _eldsssdtlgejt = value; }
        }


        private DisplayRoomFullVM _selectedroomfullvm;

        public DisplayRoomFullVM SelectedRoomFullVM
        {
            get { return _selectedroomfullvm; }
            set
            {
                if (_selectedroomfullvm != null)
                    _selectedroomfullvm.Dispose();
                if (value != null)
                {
                    _selectedroomfullvm = value;
                }
                OnPropertyChanged("SelectedRoomFullVM");
            }
        }



        private DateTime _selectedstartdate;

        public DateTime SelectedStartDate
        {
            get
            {
                return _selectedstartdate;
            }
            set
            {
                _selectedstartdate = value;
                OnPropertyChanged("SelectedStartTime");
                CombinedStartTime = new DateTime(_selectedstartdate.Year, _selectedstartdate.Month, _selectedstartdate.Day,
                SelectedStartTime.Hour, SelectedStartTime.Minute, 0);
                //RefreshRooms();
                RefreshRoomVM();
                if (SelectedRoomFullVM != null)
                    SelectedRoomFullVM.RefreshVM();
                //Adjust end date if needed
                if (SelectedEndDate <= SelectedStartDate)
                {
                    SelectedEndDate = new DateTime(SelectedEndDate.Year, SelectedStartDate.Month, SelectedStartDate.Day).AddDays(1);
                }

            }
        }

        private DateTime _selectedenddate;

        public DateTime SelectedEndDate
        {
            get
            {
                return _selectedenddate;
            }
            set
            {
                _selectedenddate = value;
                OnPropertyChanged("SelectedEndDate");
                CombinedEndTime = new DateTime(_selectedenddate.Year, _selectedenddate.Month, _selectedenddate.Day,
                SelectedEndTime.Hour, SelectedEndTime.Minute, 0);
                //RefreshRooms();
                RefreshRoomVM();
                if(SelectedRoom!=null)
                    SelectedRoomFullVM.RefreshVM();
            }
        }

        private DateTime _selectedstarttime;

        public DateTime SelectedStartTime
        {
            get
            {
                return _selectedstarttime;
            }
            set
            {
                _selectedstarttime = value;
                OnPropertyChanged("SelectedStartTime");
                CombinedStartTime = new DateTime(SelectedStartDate.Year, SelectedStartDate.Month, SelectedStartDate.Day,
                _selectedstarttime.Hour, _selectedstarttime.Minute, 0);
                //RefreshRooms();
                RefreshRoomVM();
            }
        }

        private DateTime _selectedendtime;

        public DateTime SelectedEndTime
        {
            get
            {
                return _selectedendtime;
            }
            set
            {
                _selectedendtime = value;
                OnPropertyChanged("SelectedEndTime");
                CombinedEndTime = new DateTime(SelectedEndDate.Year, SelectedEndDate.Month, SelectedEndDate.Day,
                _selectedendtime.Hour, _selectedendtime.Minute, 0);
                //RefreshRooms();
                RefreshRoomVM();
            }
        }

        private int _numpeople;

        public int PeopleCount
        {
            get { return _numpeople; }
            set
            {
                _numpeople = value;
                OnPropertyChanged("PeopleCount");
                RefreshRoomVM();
                OnChangedValues();
            }
        }


        private ObservableCollection<Floor> _floorscollection;

        public ObservableCollection<Floor> FloorsCollection
        {
            get { return _floorscollection; }
            set
            {
                _floorscollection = value;
                OnPropertyChanged("FloorsCollection");
            }
        }

        private ObservableCollection<DisplayRoomMiniVM> _roomvmcollection;

        public ObservableCollection<DisplayRoomMiniVM> RoomVMCollection
        {
            get { return _roomvmcollection; }
            set
            {
                _roomvmcollection = value;
                OnPropertyChanged("RoomVMCollection");
            }

        }


        private ICollectionView _floorcollectionview;
        public ICollectionView FloorCollectionView
        {
            get
            {
                return _floorcollectionview;
            }
            set
            {
                _floorcollectionview = value;
                OnPropertyChanged("FloorCollectionView");
            }
        }

        private List<ICollectionView> _roomvmcollectionlist;
        public List<ICollectionView> RoomVMCollectionList
        {
            get
            {
                return _roomvmcollectionlist;
            }
            set
            {
                _roomvmcollectionlist = value;
                OnPropertyChanged("RoomVMCollectionList");
            }
        }

        private ObservableCollection<RoomRowVmContainer> _vmrows;

        public ObservableCollection<RoomRowVmContainer> VmRows
        {
            get { return _vmrows; }
            set
            {
                _vmrows = value;
                OnPropertyChanged("VmRows");
            }
        }

        public UpTextBlockCommand UpTextBlockCommand { get; set; }

        public DownTextBlockCommand DownTextBlockCommand { get; set; }

        private DateTime _combinedstarttime;

        public DateTime CombinedStartTime
        {
            get { return _combinedstarttime; }
            set
            {
                _combinedstarttime = value;
                OnPropertyChanged("CombinedStartTime");
                OnChangedValues();
            }
        }

        private DateTime _combinedendtime;

        public DateTime CombinedEndTime
        {
            get { return _combinedendtime; }
            set
            {
                _combinedendtime = value;
                OnPropertyChanged("CombinedEndTime");
                OnChangedValues();
            }
        }

        private string _selectedlodging;

        public string SelectedLodging
        {
            get { return _selectedlodging; }
            set
            {
                _selectedlodging = value;
                OnPropertyChanged("SelectedLodging");
                OnChangedValues();
            }
        }

        private List<string> _lodgingtypes;

        public List<string> LodgingTypes
        {
            get { return _lodgingtypes; }
            set
            {
                _lodgingtypes = value;
                OnPropertyChanged("LodgingTypes");
            }
        }

        private Service _weeekdayhalfservice;

        public Service WeekdayHalfService
        {
            get { return _weeekdayhalfservice; }
            set { _weeekdayhalfservice = value; }
        }

        private Service _weekdayfullservice;

        public Service WeekdayFullService
        {
            get { return _weekdayfullservice; }
            set { _weekdayfullservice = value; }
        }

        private Service _weekendhalfservice;

        public Service WeekendHalfService
        {
            get { return _weekendhalfservice; }
            set { _weekendhalfservice = value; }
        }

        private Service _weekendfullservice;

        public Service WeekendFullService
        {
            get { return _weekendfullservice; }
            set { _weekendfullservice = value; }
        }



        public FloorsViewVM(ApplicationViewModel vm)
        {
            AppVm = vm;
        }

        private void InitializeFloorsWindowVM()
        {

            List<Floor> floors = SqlDatabaseHelper.Read<Floor>();
            List<Room> rooms = SqlDatabaseHelper.Read<Room>();
            List<RoomType> roomtypes = SqlDatabaseHelper.Read<RoomType>();
            List<RoomReservation> reservations = SqlDatabaseHelper.Read<RoomReservation>();
            List<Transaction> transactions = SqlDatabaseHelper.Read<Transaction>();
            List<TransactionPart> tparts = SqlDatabaseHelper.Read<TransactionPart>();
            SqlDatabaseHelper.JoinLists(transactions, tparts);
            List<Customer> customers = SqlDatabaseHelper.Read<Customer>();
            SqlDatabaseHelper.JoinLists(reservations, transactions);
            SqlDatabaseHelper.JoinDiscreteByInnerOneWay(rooms, roomtypes);
            SqlDatabaseHelper.JoinDiscreteByInner(reservations, customers);
            SqlDatabaseHelper.JoinLists(floors, rooms);
            SqlDatabaseHelper.JoinLists(rooms, reservations);
            LodgingTypes = new List<string>();
            LodgingTypes.Add("Half Pension");
            LodgingTypes.Add("Full Pension");
            List<Service> serviceTempList = SqlDatabaseHelper.Read<Service>();
            SqlDatabaseHelper.JoinDiscreteByInner(serviceTempList, SqlDatabaseHelper.Read<ServiceGroup>());
            DateTime now = DateTime.Now;
            //Load and set global parameters
            HotelGlobalParameters = ObjectFileHelper.ReadObjectFromFile<HotelGlobalParameters>
                ("PropertyFiles", "HotelGlobalParameters") as HotelGlobalParameters;

            //Set default start time to today or tommorow if after 1400, set end time by same logic

            SelectedStartTime = new DateTime
                (now.Year, now.Month, now.Day, HotelGlobalParameters.CheckInHour, HotelGlobalParameters.CheckInMinutes, 0);
            SelectedEndTime = new DateTime
                (now.Year, now.Month, now.AddDays(1).Day, HotelGlobalParameters.CheckOutHour, HotelGlobalParameters.CheckOutMinutes, 0);
            bool isTommorow = now.Hour >= HotelGlobalParameters.CheckInHour;
            if (isTommorow)
            {
                SelectedStartDate = new DateTime(now.Year, now.Month, now.Day).AddDays(1);
                SelectedEndDate = new DateTime(now.Year, now.Month, now.Day).AddDays(2);
            }
            else
            {
                SelectedStartDate = new DateTime(now.Year, now.Month, now.Day);
                SelectedEndDate = new DateTime(now.Year, now.Month, now.Day).AddDays(1);
            }

            //Set collection views to be used as sorted lists
            FloorsCollection = new ObservableCollection<Floor>(floors);
            FloorCollectionView = CollectionViewSource.GetDefaultView(FloorsCollection);
            FloorCollectionView.Filter = FilterFloors;
            FloorCollectionView.SortDescriptions.Add(new SortDescription(nameof(Floor.ElementNumber), ListSortDirection.Descending));
            RoomVMCollection = new ObservableCollection<DisplayRoomMiniVM>();
            IsRoomSelected = false;
            PeopleCount = 2;
            DownTextBlockCommand = new DownTextBlockCommand(this);
            UpTextBlockCommand = new UpTextBlockCommand(this);
            List<Service> serviceList = SqlDatabaseHelper.Read<Service>();
            WeekdayHalfService = serviceList.Where(x => x.Name == "Weekday Half Pension Night").First();
            WeekdayFullService = serviceList.Where(x => x.Name == "Weekday Full Pension Night").First();
            WeekendHalfService = serviceList.Where(x => x.Name == "Weekend Half Pension Night").First();
            WeekendFullService = serviceList.Where(x => x.Name == "Weekend Full Pension Night").First();
            VmRows = new ObservableCollection<RoomRowVmContainer>();
        }

        public void RefreshVm()
        {
            InitializeFloorsWindowVM();
        }

        private bool FilterFloors(object obj)
        {
            if (obj is Floor floor)
            {
                return floor.IsActive;
            }
            else return false;
        }

        /// <summary>
        /// Update room availability according to selected time and people count
        /// </summary>
        private void RefreshRoomVM()
        {
            if (VmRows != null)
            {
                foreach (var row in VmRows)
                {
                    foreach (DisplayRoomMiniVM vm in row.CollectionView)
                    {
                        vm.RefreshRoomVM(CombinedStartTime, CombinedEndTime, PeopleCount);
                    }
                }
            }
        }


        private void RefreshUnavailableView()
        {
            if (SelectedFloor != null)
            {
                foreach (RoomRowVmContainer row in VmRows)
                {
                    row.SetFilter(IsUnavailableChecked);
                }
            }
            OnPropertyChanged("VmRows");
        }

        public void Refresh()
        {
            RefreshVm();
            OnPropertyChanged("VM");
        }

        public void Dispose()
        {
            Debug.WriteLine("FloorView Dispose");
            VmRows.Clear();
            FloorsCollection.Clear();
            RoomVMCollection.Clear();
            SelectedFloor = null;
            SelectedRoom = null;
        }

        public override void IncrementProperty()
        {
            PeopleCount++;
        }

        public override void DecrementProperty()
        {
            PeopleCount--;
        }

        //Combine date and time of day
        public DateTime GetStartTime()
        {
            return new DateTime(SelectedStartDate.Year, SelectedStartDate.Month, SelectedStartDate.Day,
                SelectedStartTime.Hour, SelectedStartTime.Minute, 0);
        }

        //Combine date and time of day
        public DateTime GetEndTime()
        {
            return new DateTime(SelectedEndDate.Year, SelectedEndDate.Month, SelectedEndDate.Day,
                SelectedEndTime.Hour, SelectedEndTime.Minute, 0);
        }

        public void OnChangedValues()
        {
            //TODO
            if (SelectedRoom != null)
            {
                SelectedRoomFullVM.RefreshVM();
            }
        }
    }
}