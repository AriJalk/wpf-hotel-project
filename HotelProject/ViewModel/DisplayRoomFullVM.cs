using HotelProject.Model.DbClasses;
using HotelProject.View;
using HotelProject.ViewModel.Commands.RoomCommands;
using HotelProject.ViewModel.Helpers;
using HotelProject.ViewModel.Helpers.Pdf;
using System;
using System.Collections;
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
{
    public class DisplayRoomFullVM : ViewModelBase
    {
        private FloorsViewVM _parentvm;

        public FloorsViewVM ParentVm
        {
            get { return _parentvm; }
            set { _parentvm = value; }
        }

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
                OnPropertyChanged("Room");
            }
        }

        private Customer _selectedcustomer;

        public Customer SelectedCustomer
        {
            get
            {
                return _selectedcustomer;
            }
            set
            {
                _selectedcustomer = value;
                OnPropertyChanged("SelectedCustomer");
            }
        }

        private RoomReservation _backupreservation;

        private RoomReservation _selectedreservation;

        public RoomReservation SelectedReservation
        {
            get { return _selectedreservation; }
            set
            {
                bool valid = false;
                //Save or revert object
                if (_selectedreservation != null)
                {
                    if (_selectedreservation.ValidateData())
                    {
                        SqlDatabaseHelper.Insert(_selectedreservation);
                        _selectedreservation.IsInDb = true;
                        valid = true;
                    }
                    else
                    {
                        if (_selectedreservation.IsInDb)
                        {
                            MessageBox.Show("Info not correct, reverting");
                            _selectedreservation = _backupreservation;

                        }
                        //Delete row/object
                        else
                        {
                            _selectedreservation.IdCount--;
                            _selectedreservation = null;
                        }
                        RefreshVM();
                    }
                }
                if (value != null && valid)
                {
                    _selectedreservation = value;
                    _backupreservation = new RoomReservation(value);
                }
                OnPropertyChanged("SelectedReservation");
            }
        }

        private DateTime _startime;

        public DateTime StartTime
        {
            get { return _startime; }
            set
            {
                _startime = value;
                OnPropertyChanged("StartTime");
            }
        }

        private DateTime _endtime;

        public DateTime EndTime
        {
            get { return _endtime; }
            set
            {
                _endtime = value;
                OnPropertyChanged("EndTime");
            }
        }

        private bool _isroomavailable;

        public bool IsRoomAvailable
        {
            get { return _isroomavailable; }
            set
            {
                _isroomavailable = value;
                OnPropertyChanged("IsRoomAvailable");
            }
        }

        private ObservableCollection<TransactionPart> _transactionpartlist;

        public ObservableCollection<TransactionPart> TransactionPartList
        {
            get { return _transactionpartlist; }
            set
            {
                _transactionpartlist = value;
                OnPropertyChanged("TransactionPartList");
            }
        }


        public NewReservationCommand NewReservationCommand { get; set; }

        private ObservableCollection<RoomReservation> _roomreservations;

        public ObservableCollection<RoomReservation> RoomReservations
        {
            get { return _roomreservations; }
            set
            {
                _roomreservations = value;
                OnPropertyChanged("RoomReservationList");
            }
        }


        public string CustomerName
        {
            get
            {
                if(SelectedCustomer!=null)
                    return SelectedCustomer.FName + " " + SelectedCustomer.LName;
                return string.Empty;
            }
        }

        private ICollectionView _roomreservationscollection;

        public ICollectionView RoomReservationsCollection
        {
            get { return _roomreservationscollection; }
            set
            {
                _roomreservationscollection = value;
                OnPropertyChanged("RoomReservationsCollection");
            }
        }

        private string Lodging;

        private List<TransactionPart> _partlist;

        public List<TransactionPart> PartList
        {
            get { return _partlist; }
            set
            {
                _partlist = value;
                OnPropertyChanged("PartList");
            }
        }

        private Transaction NewTransaction = null;

        private RoomReservation NewReservation = null;

        public DisplayRoomFullVM(FloorsViewVM vm)
        {
            Room = vm.SelectedRoom;
            _selectedcustomer = vm.AppVm.Globals.SelectedCustomer;
            ParentVm = vm;
            NewReservationCommand = new NewReservationCommand(this);
            RefreshVM();
        }


        public void RefreshVM()
        {
            PartList = null;
            Lodging = ParentVm.SelectedLodging;
            StartTime = ParentVm.CombinedStartTime;
            EndTime = ParentVm.CombinedEndTime;
            RoomReservations = new ObservableCollection<RoomReservation>(Room.RoomReservationList);
            RoomReservationsCollection = CollectionViewSource.GetDefaultView(RoomReservations);
            RoomReservationsCollection.Filter = ReservationFilter;
            RoomReservationsCollection.SortDescriptions.Add(new SortDescription(nameof(RoomReservation.StartTime), ListSortDirection.Descending));
            if (ParentVm.AppVm.Globals.User.UserType.Name == "Manager" || ParentVm.AppVm.Globals.User.UserType.Name == "Admin")
                IsRoomAvailable = Room.IsRoomAvailable(StartTime, EndTime, ParentVm.PeopleCount)
                    && ParentVm.AppVm.Globals.SelectedCustomer!=null
                    && Lodging != null;
            else IsRoomAvailable =
                    Room.IsRoomAvailable(StartTime, EndTime, ParentVm.PeopleCount)
                    && StartTime >= DateTime.Now
                    && EndTime >= DateTime.Now
                    && ParentVm.AppVm.Globals.SelectedCustomer!=null
                    && Lodging != null;
            if (IsRoomAvailable)
                InitializeNewReservation();
        }

        private void InitializeNewReservation()
        {

            NewReservation = new RoomReservation(Room, StartTime, EndTime, SelectedCustomer, ParentVm.PeopleCount);
            NewTransaction = new Transaction(NewReservation, ParentVm.AppVm.Globals.User,"Lodging");
            PartList = new List<TransactionPart>();
            NewReservation.TransactionList.Add(NewTransaction);
            int nights = (EndTime.Date - StartTime.Date).Days;
            for (int i = 0; i < nights; i++)
            {
                TransactionPart newPart = null;

                if ((int)StartTime.AddDays(i).DayOfWeek >= 0 && (int)StartTime.AddDays(i).DayOfWeek <= 4)
                {
                    if (ParentVm.SelectedLodging == "Half Pension")
                        newPart = new TransactionPart(NewTransaction, ParentVm.WeekdayHalfService, Room.RoomType, ParentVm.PeopleCount);
                    else if (ParentVm.SelectedLodging == "Full Pension")
                        newPart = new TransactionPart(NewTransaction, ParentVm.WeekdayFullService, Room.RoomType, ParentVm.PeopleCount);

                }
                else
                {
                    if (ParentVm.SelectedLodging == "Half Pension")
                        newPart = new TransactionPart(NewTransaction, ParentVm.WeekendHalfService, Room.RoomType, ParentVm.PeopleCount);
                    else if (ParentVm.SelectedLodging == "Full Pension")
                        newPart = new TransactionPart(NewTransaction, ParentVm.WeekendFullService, Room.RoomType, ParentVm.PeopleCount);
                }
                if (newPart != null)
                {
                    //Set temp id without changing the idcount if the order is discarded
                    newPart.SetTempId(newPart.IdCount + i + 1);
                    PartList.Add(newPart);
                }

            }


            if (PartList != null)
            {
                NewTransaction.TransactionPartList = PartList;

                TransactionPartList = new ObservableCollection<TransactionPart>(PartList);
            }
        }

        private bool ReservationFilter(object obj)
        {
            if (obj is RoomReservation res)
            {
                if (res.IsActive)
                    return true;
            }
            return false;
        }

        public void OpenCreditView()
        {
            CreditCardView window = new CreditCardView();
            CreditCardViewVM vm = new CreditCardViewVM(NewTransaction, window);
            window.DataContext = vm;
            window.ShowDialog();
            if (NewTransaction.IsPayed)
            {
                SqlDatabaseHelper.Insert(NewTransaction);
                foreach (TransactionPart part in NewTransaction.TransactionPartList)
                {
                    SqlDatabaseHelper.Insert(part);
                    part.IdCount++;
                }
                NewTransaction.IdCount++;
                SqlDatabaseHelper.Insert(NewReservation);
                NewReservation.IdCount++;
                Room.RoomReservationList.Add(NewReservation);
                ParentVm.SelectedRoomMiniVM.IsRoomAvailable = false;
                HtmlReports.ReservationTicket(NewReservation, ParentVm.AppVm.User);
                RefreshVM();
            }
        }

        public void Dispose()
        {


        }
    }


}
