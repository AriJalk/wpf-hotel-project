using HotelProject.Model.BaseClasses;
using HotelProject.Model.Helpers;
using HotelProject.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelProject.Model.DbClasses
{
    /// <summary>
    /// Represents an hotel floor with a list of rooms
    /// </summary>
    public class Floor : HotelNumberedElementBase, IIncremented, IComparable<Floor>
    {
        public static new Dictionary<string, string> Fields { get; set; }

        private static int _idcount = 0;

        public override int IdCount
        {
            get { return _idcount; }

            set { _idcount = value; }
        }

        private int _floorid;
        /// <summary>
        /// DB Property
        /// Primary key
        /// Unique floor id
        /// </summary>
        public int FloorId
        {
            get { return _floorid; }
            set 
            { 
                _floorid = value;
                if (_floorid > IdCount)
                    IdCount = _floorid;

            }
        }

        private List<Room> _roomlist;
        /// <summary>
        /// List of rooms in floor
        /// </summary>
        public List<Room> RoomList
        {
            get { return _roomlist; }
            set { _roomlist = value; }
        }
        /// <summary>
        /// Copy CTOR
        /// </summary>
        /// <param name="other"></param>
        public Floor(Floor other)
        {
            this.CreatedTime = other.CreatedTime;
            this.ElementNumber = other.ElementNumber;
            this.FloorId = other.FloorId;
            this.IsActive = other.IsActive;
            this.IsInDb = other.IsInDb;
            this.RoomList = other.RoomList;
        }

        /// <summary>
        /// Basic only info CTOR
        /// </summary>
        /// <param name="num"></param>
        public Floor(int num) :base(num)
        {
            RoomList = new List<Room>();
            FloorId = ++IdCount;
        }

        /// <summary>
        /// Floor CTOR for full initialization
        /// </summary>
        /// <param name="floorid"></param>
        /// <param name="num"></param>
        /// <param name="rooms"></param>
        public Floor(int num,List<Room> rooms) :base(num)
        {
            FloorId = ++IdCount;
            RoomList = rooms;
        }

        /// <summary>
        /// Empty parameter CTOR
        /// </summary>
        public Floor()
        {
            RoomList = new List<Room>();
        }

        static Floor()
        {
            Fields = new Dictionary<string, string>(HotelNumberedElementBase.Fields);
            Fields.Add("FloorId", "INT NOT NULL");
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
            return FloorId;
        }

        public override string GetPrimaryKeyType()
        {
            return "FloorId";
        }

        public override void SetPrimaryKey(int value)
        {
            FloorId = value;
        }

        public override List<string> GetTableTemplate()
        {
            List<string> template = base.GetTableTemplate();
            template.Add(TableFieldFormat(GetPrimaryKeyType(),Fields[GetPrimaryKeyType()]+" PRIMARY KEY"));
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
            if (ElementNumber > 0)
                IsInDb = true;
        }

        /// <summary>
        /// Compares floors by revenue for sort
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Floor other)
        {
            decimal totalA = 0;
            decimal totalB = 0;
            foreach (Room room in RoomList)
            {
                
                foreach(RoomReservation reservation in room.RoomReservationList)
                {
                    foreach (Transaction transaction in reservation.TransactionList)
                        if (transaction.IsPayed && !transaction.IsRefunded)
                            totalA += transaction.ToPayAmount;
                }
            }
            foreach(Room room in other.RoomList)
            {
                foreach (RoomReservation reservation in room.RoomReservationList)
                {
                    foreach (Transaction transaction in reservation.TransactionList)
                        if (transaction.IsPayed && !transaction.IsRefunded)
                            totalB += transaction.ToPayAmount;
                }
            }
            if (totalA > totalB)
                return -1;
            if (totalA < totalB)
                return 1;
            else return 0;
        }
    }
}
