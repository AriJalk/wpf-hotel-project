using HotelProject.Model.BaseClasses;
using HotelProject.Model.Helpers;
using HotelProject.Model.Interfaces;
using System.Collections.Generic;

namespace HotelProject.Model.DbClasses
{
    /// <summary>
    /// Represents a customer to link to reservations
    /// </summary>
    public class Customer : Person, IIncremented
    {
        public static new Dictionary<string, string> Fields { get; set; }

        static private int _idcount = 0;
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


        private int _customerid;
        /// <summary>
        /// DB Property
        /// Primary Key
        /// </summary>
        public int CustomerId
        {
            get { return _customerid; }
            set 
            {
                _customerid = value;
                if (_customerid > IdCount)
                    IdCount = _customerid;
            }
        }

        private List<RoomReservation> _roomreservationlist;

        public List<RoomReservation> RoomReservationList
        {
            get { return _roomreservationlist; }
            set { _roomreservationlist = value; }
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="source"></param>
        public Customer(Customer source) : base(source.FName, source.LName, source.PhoneNumber, source.IdNumber)
        {
            CustomerId = source.CustomerId;
            CreatedTime = source.CreatedTime;
        }

        /// <summary>
        /// Initialize with person
        /// </summary>
        /// <param name="person"></param>
        public Customer(Person person)
        {
            FName = person.FName;
            LName = person.LName;
            PhoneNumber = person.PhoneNumber;
            CustomerId = ++IdCount;
            IdNumber = person.IdNumber;
        }

        //TODO: REMOVE
        public Customer(bool dummy)
        {

        }

        public Customer()
        {
            CustomerId = ++IdCount;
        }

        static Customer()
        {
            Fields = new Dictionary<string, string>(Person.Fields);
            Fields.Add("CustomerId", "INT NOT NULL PRIMARY KEY");
        }

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
            return CustomerId;
        }

        public override string GetPrimaryKeyType()
        {
            return "CustomerId";
        }

        public override void SetPrimaryKey(int value)
        {
            CustomerId = value;
        }

        public override List<string> GetTableTemplate()
        {
            List<string> template = base.GetTableTemplate();
            template.Add(TableFieldFormat(GetPrimaryKeyType(), Fields[GetPrimaryKeyType()]));
            return template;
        }

        public override List<TableData> GetValues()
        {
            List<TableData> values = base.GetValues();
            values.Add(new TableData(GetPrimaryKey().ToString(), GetPrimaryKeyType()));
            return values;
        }

        public override Dictionary<string, string> GetFields()
        {
            return Fields;
        }

        public override void SetInDb()
        {
            if (FName != null && LName != null)
                IsInDb = true;
        }
    }
}
