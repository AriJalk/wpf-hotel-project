using HotelProject.Model.BaseClasses;
using HotelProject.Model.Helpers;
using HotelProject.Model.Interfaces;
using HotelProject.ViewModel.Helpers;
using System;
using System.Collections.Generic;

namespace HotelProject.Model.DbClasses
{
    /// <summary>
    /// Represents Room Reservation orders
    /// </summary>
    public class RoomReservation : HotelDbElementBase, IIncremented
    {
        public static new Dictionary<string, string> Fields { get; set; }

        private static int _idcount = 0;

        public override int IdCount
        { 
            get 
            { 
                return _idcount;
            }  
            set 
            { 
                _idcount = value; 
            }  
        }

        private int _roomreservationid;
        /// <summary>
        /// DB Property 
        /// Primary Key
        /// </summary>
        public int RoomReservationId
        {
            get { return _roomreservationid; }
            set
            {
                _roomreservationid = value;
                if (_roomreservationid > IdCount)
                    IdCount = _roomreservationid;
            }
        }


        private int _roomid;
        //DB Property
        public int RoomId
        {
            get { return _roomid; }
            set { _roomid = value; }
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
                RoomId = _room.GetPrimaryKey();
            }
        }

        private DateTime _starttime;
        /// <summary>
        /// DB Property
        /// DATETIME
        /// </summary>
        public DateTime StartTime
        {
            get { return _starttime; }
            set { _starttime = value; }
        }

        private DateTime _endtime;
        /// <summary>
        /// DB Property
        /// </summary>
        public DateTime EndTime
        {
            get { return _endtime; }
            set { _endtime = value; }
        }

        private List<Transaction> _transactionlist;
        /// <summary>
        /// Transactions linked with Reservation, allows for multiple transactions for the same order
        /// but only one transaction is for the reservation itself
        /// </summary>
        public List<Transaction> TransactionList
        {
            get
            {
                return _transactionlist;
            }
            set
            {
                if (value != null)
                {
                    _transactionlist = value;
                }
            }
        }

        private int _peoplecount;
        /// <summary>
        /// DB Property
        /// how many people
        /// </summary>
        public int PeopleCount
        {
            get { return _peoplecount; }
            set { _peoplecount = value; }
        }


        private int _customerid;
        //DB Property
        public int CustomerId
        {
            get { return _customerid; }
            set { _customerid = value; }
        }

        private bool _ischeckin;
        //DB Property
        public bool IsCheckIn
        {
            get { return _ischeckin; }
            set { _ischeckin = value; }
        }

        private bool _ischeckout;
        //DB Property
        public bool IsCheckOut
        {
            get { return _ischeckout; }
            set { _ischeckout = value; }
        }


        private Customer _customer;
        /// <summary>
        /// Customer linked with reservation
        /// </summary>
        public Customer Customer
        {
            get { return _customer; }
            set
            {
                if (value != null)
                {
                    _customer = value;
                    CustomerId = _customer.CustomerId;
                }
            }
        }

        /// <summary>
        /// Copy CTOR
        /// </summary>
        /// 
        public RoomReservation(RoomReservation source)
        {
            this.CreatedTime = source.CreatedTime;
            this.Customer = source.Customer;
            this.EndTime = source.EndTime;
            this.IsActive = source.IsActive;
            this.StartTime = source.StartTime;
            this.TransactionList = source.TransactionList;
            this.RoomReservationId = source.RoomReservationId;
        }


        public RoomReservation()
        {
            TransactionList = new List<Transaction>();
        }


        public RoomReservation(Room room, DateTime start, DateTime end, Customer customer, int peopleCount)
        {
            _roomreservationid = IdCount + 1;
            Room = room;
            StartTime = start;
            EndTime = end;
            Customer = customer;
            PeopleCount = peopleCount;
            TransactionList = new List<Transaction>();
        }

        static RoomReservation()
        {
            Fields = new Dictionary<string, string>(HotelDbElementBase.Fields);
            Fields.Add("RoomReservationId", "INT NOT NULL UNIQUE");
            Fields.Add("RoomId", "INT NOT NULL");
            Fields.Add("CustomerId", "INT NOT NULL");
            Fields.Add("PeopleCount", "INT NOT NULL");
            Fields.Add("StartTime", "DATETIME NOT NULL");
            Fields.Add("EndTime", "DATETIME NOT NULL");
            Fields.Add("IsCheckIn", "BIT NOT NULL");
            Fields.Add("IsCheckOut", "BIT NOT NULL");
        }

        #region Methods
        public static int GetIdCount()
        {
            return _idcount;
        }

        public static void SetIdCount(int value)
        {
            _idcount = value;
        }

        public override int GetPrimaryKey()
        {
            return RoomReservationId;
        }

        public override string GetPrimaryKeyType()
        {
            return "RoomReservationId";
        }

        public override void SetPrimaryKey(int value)
        {
            RoomReservationId = value;
        }

        public override List<string> GetTableTemplate()
        {
            List<string> template = base.GetTableTemplate();
            template.Add(TableFieldFormat(GetPrimaryKeyType(), Fields[GetPrimaryKeyType()]));
            template.Add(TableFieldFormat("RoomId", Fields["RoomId"]));
            template.Add(TableFieldFormat("CustomerId", Fields["CustomerId"]));
            template.Add(TableFieldFormat("PeopleCount", Fields["PeopleCount"]));
            template.Add(TableFieldFormat("StartTime", Fields["StartTime"]));
            template.Add(TableFieldFormat("EndTime", Fields["EndTime"]));
            template.Add(TableFieldFormat("IsCheckIn", Fields["IsCheckIn"]));
            template.Add(TableFieldFormat("IsCheckOut", Fields["IsCheckOut"]));
            return template;
        }

        public override List<TableData> GetValues()
        {
            List<TableData> values = base.GetValues();
            values.Add(new TableData(GetPrimaryKey().ToString(), GetPrimaryKeyType()));
            values.Add(new TableData(RoomId.ToString(), "RoomId"));
            values.Add(new TableData(CustomerId.ToString(), "CustomerId"));
            values.Add(new TableData(PeopleCount.ToString(), "PeopleCount"));
            values.Add(new TableData(StartTime.ToString(), "StartTime"));
            values.Add(new TableData(EndTime.ToString(), "EndTime"));
            values.Add(new TableData(Converters.BoolToTable(IsCheckIn), "IsCheckIn"));
            values.Add(new TableData(Converters.BoolToTable(IsCheckOut), "IsCheckOut"));
            return values;
        }
        public override Dictionary<string, string> GetFields()
        {
            return Fields;
        }
        public override List<string> GenerateErrors()
        {
            List<string> errors = base.GenerateErrors();
            /*if (Customer == null)
                errors.Add("Customer not valid");
            if (Room == null)
                errors.Add("Room not valid");*/
            if (TransactionList[0].RoomReservationId != this.RoomReservationId)
                errors.Add("Transaction not valid");
            if (IsCheckIn && StartTime > DateTime.Now)
                errors.Add("Check-In too early");
            if (IsCheckOut && !IsCheckIn)
                errors.Add("Check-Out without Check-In");
            return errors;
        }

        public override void SetInDb()
        {
            if (RoomId > 0)
                IsInDb = true;
        }
        #endregion Methods


    }
}
