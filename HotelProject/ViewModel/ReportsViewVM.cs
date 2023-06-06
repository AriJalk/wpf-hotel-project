using HotelProject.Model.DbClasses;
using HotelProject.ViewModel.Commands.Reports;
using HotelProject.ViewModel.Helpers;
using HotelProject.ViewModel.Helpers.Pdf;
using System;
using System.Collections.Generic;

namespace HotelProject.ViewModel
{
    /// <summary>
    /// Reports VM, Uses HTMLReports class to produce reports
    /// </summary>
    class ReportsViewVM : ViewModelBase, IPageViewModel
    {
        public string Name => "Reports";

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

        private Customer _selectedcustomer;

        public Customer SelectedCustomer
        {
            get { return _selectedcustomer; }
            set 
            { 
                _selectedcustomer = value;
                OnPropertyChanged("SelectedCustomer");
            }
        }


        private AllReservationsReportCommand _allreservationsreportcommand;

        public AllReservationsReportCommand AllReservationsReportCommand
        {
            get { return _allreservationsreportcommand; }
            set
            { 
                _allreservationsreportcommand = value;
                OnPropertyChanged("AllReservationsReportCommand");
            }
        }

        private AllCustomersReportCommand _allcustomerscommand;

        public AllCustomersReportCommand AllCustomersReportCommand
        {
            get { return _allcustomerscommand; }
            set
            { 
                _allcustomerscommand = value;
                OnPropertyChanged("AllCustomersCommand");
            }
        }

        private AllRoomsReportCommand _allroomsreportcommand;

        public AllRoomsReportCommand AllRoomsReportCommand
        {
            get { return _allroomsreportcommand; }
            set
            { 
                _allroomsreportcommand = value;
                OnPropertyChanged("AllRoomsReportCommand");
            }
        }

        private AllFloorsReportCommand _allfloorsreportcommand;

        public AllFloorsReportCommand AllFloorsReportCommand
        {
            get { return _allfloorsreportcommand; }
            set { _allfloorsreportcommand = value; }
        }

        private ReservationsByCustomerCommand _reservationsByCustomerCommand;

        public ReservationsByCustomerCommand ReservationsByCustomerCommand
        {
            get { return _reservationsByCustomerCommand; }
            set 
            {
                _reservationsByCustomerCommand = value;
                OnPropertyChanged("ReservationsByCustomerCommand");
            }
        }

        private Room _selectedroom;

        public Room SelectedRoom
        {
            get { return _selectedroom; }
            set
            {
                if (value != null)
                    _selectedroom = value;
            }
        }
        private Floor _selectedfloor;

        public Floor SelectedFloor
        {
            get { return _selectedfloor; }
            set
            {
                if (value != null)
                    _selectedfloor = value;
            }
        }

        private DateTime _startdate;

        public DateTime StartDate
        {
            get { return _startdate; }
            set { _startdate = value; }
        }

        private DateTime _enddate;

        public DateTime EndDate
        {
            get { return _enddate; }
            set { _enddate = value; }
        }



        public ReportsViewVM(ApplicationViewModel vm)
        {
            AppVm = vm;
            AllReservationsReportCommand = new AllReservationsReportCommand(this);
            AllCustomersReportCommand = new AllCustomersReportCommand(this);
            AllRoomsReportCommand = new AllRoomsReportCommand(this);
            AllFloorsReportCommand = new AllFloorsReportCommand(this);
            StartDate = DateTime.Now.AddMonths(-6);
            EndDate = DateTime.Now;
        }



        public void AllReservationsReport()
        {
            //Load and merge all tables
            List<RoomReservation> reservations = SqlDatabaseHelper.Read<RoomReservation>
                ($"StartTime BETWEEN #{StartDate.ToString("MM/dd/yyyy")}# AND #{EndDate.AddDays(1).ToString("MM/dd/yyyy")}#");
            List<Transaction> transactions = SqlDatabaseHelper.Read<Transaction>();
            transactions.AddRange(SqlDatabaseHelper.ReadArchive<Transaction>());
            List<TransactionPart> transactionParts = SqlDatabaseHelper.Read<TransactionPart>();
            transactionParts.AddRange(SqlDatabaseHelper.ReadArchive<TransactionPart>());
            List<ServiceGroup> servicegroups = SqlDatabaseHelper.Read<ServiceGroup>();
            List<Service> services = SqlDatabaseHelper.Read<Service>();
            List<Customer> customers = SqlDatabaseHelper.Read<Customer>();
            List<User> users = SqlDatabaseHelper.Read<User>();
            List<Room> rooms = SqlDatabaseHelper.Read<Room>();
            List<RoomType> roomtypes = SqlDatabaseHelper.Read<RoomType>();
            List<Floor> floors = SqlDatabaseHelper.Read<Floor>();
            SqlDatabaseHelper.JoinLists(floors, rooms);
            SqlDatabaseHelper.JoinDiscreteByInnerOneWay(rooms, roomtypes);
            SqlDatabaseHelper.JoinDiscreteByInnerOneWay(services, servicegroups);
            SqlDatabaseHelper.JoinDiscreteByInnerOneWay(reservations, customers);
            SqlDatabaseHelper.JoinLists(reservations, transactions);
            SqlDatabaseHelper.JoinDiscreteByInnerOneWay(reservations, rooms);
            SqlDatabaseHelper.JoinLists(transactions, transactionParts);
            SqlDatabaseHelper.JoinDiscreteByInnerOneWay(transactions, users);
            SqlDatabaseHelper.JoinDiscreteByInnerOneWay(transactionParts, services);
            HtmlReports.AllReservations(reservations,AppVm.User,StartDate,EndDate);
        }

        public void AllCustomersReport()
        {
            List<Customer> customerList = SqlDatabaseHelper.Read<Customer>();
            List<Transaction> transactionList = SqlDatabaseHelper.Read<Transaction>();
            transactionList.AddRange(SqlDatabaseHelper.ReadArchive<Transaction>());
            List<RoomReservation> reservationList = SqlDatabaseHelper.Read<RoomReservation>();
            SqlDatabaseHelper.JoinLists(reservationList, transactionList);
            SqlDatabaseHelper.JoinLists(customerList, reservationList);
            HtmlReports.AllCustomersReport(customerList,AppVm.User);
        }

        public void AllRoomsReport()
        {
            List<RoomType> roomTypeList = SqlDatabaseHelper.Read<RoomType>();
            List<Room> roomList = SqlDatabaseHelper.Read<Room>();
            List<RoomReservation> reservationList = SqlDatabaseHelper.Read<RoomReservation>
                ($"StartTime BETWEEN #{StartDate.ToString("MM/dd/yyyy")}# AND #{EndDate.AddDays(1).ToString("MM/dd/yyyy")}#");
            List<Floor> floorList = SqlDatabaseHelper.Read<Floor>();
            List<Transaction> transactionList = SqlDatabaseHelper.Read<Transaction>();
            transactionList.AddRange(SqlDatabaseHelper.ReadArchive<Transaction>());
            SqlDatabaseHelper.JoinDiscreteByInnerOneWay(roomList, floorList);
            SqlDatabaseHelper.JoinLists(reservationList, transactionList);
            SqlDatabaseHelper.JoinDiscreteByInnerOneWay(roomList, roomTypeList);
            SqlDatabaseHelper.JoinLists(roomList, reservationList);
            roomList.Sort();
            HtmlReports.AllRoomsReport(roomList,AppVm.User,StartDate,EndDate);
        }

        public void AllFloorsReport()
        {
            List<RoomType> roomTypeList = SqlDatabaseHelper.Read<RoomType>();
            List<Room> roomList = SqlDatabaseHelper.Read<Room>();
            List<RoomReservation> reservationList = SqlDatabaseHelper.Read<RoomReservation>
                ($"StartTime BETWEEN #{StartDate.ToString("MM/dd/yyyy")}# AND #{EndDate.AddDays(1).ToString("MM/dd/yyyy")}#");
            List<Floor> floorList = SqlDatabaseHelper.Read<Floor>();
            List<Transaction> transactionList = SqlDatabaseHelper.Read<Transaction>();
            transactionList.AddRange(SqlDatabaseHelper.ReadArchive<Transaction>());
            SqlDatabaseHelper.JoinLists(floorList, roomList);
            SqlDatabaseHelper.JoinLists(reservationList, transactionList);
            SqlDatabaseHelper.JoinDiscreteByInnerOneWay(roomList, roomTypeList);
            SqlDatabaseHelper.JoinLists(roomList, reservationList);
            floorList.Sort();
            HtmlReports.AllFloorsReport(floorList,AppVm.User,StartDate,EndDate);
        }

        public void ReservationsByCustomerReport()
        {
            if (SelectedCustomer != null)
            {
                List<RoomReservation> reservations = SqlDatabaseHelper.Read<RoomReservation>
                    ($"StartTime BETWEEN #{StartDate.ToString("MM/dd/yyyy")}# AND #{EndDate.AddDays(1).ToString("MM/dd/yyyy")}#");
                List<Transaction> transactions = SqlDatabaseHelper.Read<Transaction>();
                transactions.AddRange(SqlDatabaseHelper.ReadArchive<Transaction>());
                List<TransactionPart> transactionParts = SqlDatabaseHelper.Read<TransactionPart>();
                transactionParts.AddRange(SqlDatabaseHelper.ReadArchive<TransactionPart>());
                List<ServiceGroup> servicegroups = SqlDatabaseHelper.Read<ServiceGroup>();
                List<Service> services = SqlDatabaseHelper.Read<Service>();
                List<Customer> customers = SqlDatabaseHelper.Read<Customer>();
                List<User> users = SqlDatabaseHelper.Read<User>();
                List<Room> rooms = SqlDatabaseHelper.Read<Room>();
                List<RoomType> roomtypes = SqlDatabaseHelper.Read<RoomType>();
                List<Floor> floors = SqlDatabaseHelper.Read<Floor>();
                SqlDatabaseHelper.JoinLists(floors, rooms);
                SqlDatabaseHelper.JoinDiscreteByInnerOneWay(rooms, roomtypes);
                SqlDatabaseHelper.JoinDiscreteByInnerOneWay(services, servicegroups);
                SqlDatabaseHelper.JoinDiscreteByInnerOneWay(reservations, customers);
                SqlDatabaseHelper.JoinLists(reservations, transactions);
                SqlDatabaseHelper.JoinDiscreteByInnerOneWay(reservations, rooms);
                SqlDatabaseHelper.JoinLists(transactions, transactionParts);
                SqlDatabaseHelper.JoinDiscreteByInnerOneWay(transactions, users);
                SqlDatabaseHelper.JoinDiscreteByInnerOneWay(transactionParts, services);
                SelectedCustomer.RoomReservationList = new List<RoomReservation>();
                foreach (RoomReservation reservation in reservations)
                {
                    if (reservation.CustomerId == SelectedCustomer.CustomerId)
                    {
                        SelectedCustomer.RoomReservationList.Add(reservation);
                    }
                }

                HtmlReports.ReservationsByCustomer(SelectedCustomer, AppVm.User,StartDate,EndDate);

            }
        }

        public void Dispose()
        {
           
        }

        public void Refresh()
        {
            SelectedCustomer = AppVm.Globals.SelectedCustomer;
            ReservationsByCustomerCommand = new ReservationsByCustomerCommand(this);
            SelectedFloor = AppVm.Globals.SelectedFloor;
            SelectedRoom = AppVm.Globals.SelectedRoom;
        }
    }
}
