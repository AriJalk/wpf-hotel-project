using HotelProject.Model.BaseClasses;
using HotelProject.Model.Helpers;
using HotelProject.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace HotelProject.Model.DbClasses
{
    /// <summary>
    /// Represents a single room in a floor
    /// </summary>
    public class Room : HotelNumberedElementBase, IIncremented, IComparable<Room>
    {
        public static new Dictionary<string, string> Fields { get; set; }

        private static int _idcount = 0;
        public override int IdCount { get => _idcount; set => _idcount = value; }

        private int _roomid;
        /// <summary>
        /// DB Property
        /// Primary Key
        /// </summary>
        public int RoomId
        {
            get { return _roomid; }
            set 
            {
                _roomid = value;
                if (_roomid > IdCount)
                    IdCount = _roomid;
            }
        }


        private int _floorid;
        /// <summary>
        /// DB Property
        /// </summary>
        public int FloorId
        {
            get { return _floorid; }
            set { _floorid = value; }
        }

        private int _row;
        /// <summary>
        /// Room row, will be displayed in UI accordingly
        /// </summary>
        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }



        private int _roomtypeid;
        /// <summary>
        /// DB Property
        /// </summary>
        public int RoomTypeId
        {
            get { return _roomtypeid; }
            set { _roomtypeid = value; }
        }


        private int _peoplecount;
        /// <summary>
        /// DB Property
        /// </summary>
        public int MaxCapacity
        {
            get { return _peoplecount; }
            set { _peoplecount = value; }
        }

        private List<RoomReservation> _roomreservationlist;
        /// <summary>
        /// List of all reservations linked with the room
        /// </summary>
        public List<RoomReservation> RoomReservationList
        {
            get { return _roomreservationlist; }
            set { _roomreservationlist = value; }
        }


        private RoomType _roomtype;
        /// <summary>
        /// RoomType of room
        /// </summary>
        public RoomType RoomType
        {
            get
            {
                return _roomtype;
            }
            set
            {
                if (value != null)
                {
                    _roomtype = value;
                    RoomTypeId = _roomtype.GetPrimaryKey();
                }
            }
        }


        private Floor _floor;
        /// <summary>
        /// The floor containing the room
        /// </summary>
        public Floor Floor
        {
            get
            {
                if (_floor != null)
                    return _floor;
                return null;
            }
            set
            {
                _floor = value;
                FloorId = _floor.GetPrimaryKey();
            }
        }

        /// <summary>
        /// Copy CTOR
        /// </summary>
        /// <param name="other"></param>
        public Room(Room other)
        {
            this.CreatedTime = other.CreatedTime;
            this.ElementNumber = other.ElementNumber;
            this.Floor = other.Floor;
            this.IsActive = other.IsActive;
            this.IsInDb = other.IsInDb;
            this.MaxCapacity = other.MaxCapacity;
            this.RoomId = other.RoomId;
            this.RoomReservationList = other.RoomReservationList;
            this.RoomType = other.RoomType;
            this.Row = other.Row;
        }


        /// <summary>
        /// Basic only info CTOR
        /// </summary>
        /// <param name="num"></param>
        /// <param name="type"></param>
        /// <param name="floor"></param>
        public Room(int num, RoomType roomtype, Floor floor, int peoplecount, int row) : base(num)
        {
            RoomType = roomtype;
            Floor = floor;
            RoomId = ++IdCount;
            MaxCapacity = peoplecount;
            Row = row;
        }

        public Room() 
        {
            RoomReservationList = new List<RoomReservation>();
        }


        static Room()
        {
            Fields = new Dictionary<string, string>(HotelNumberedElementBase.Fields)
            {
                { "RoomId", "INT NOT NULL" },
                { "FloorId", "INT NOT NULL" },
                { "Row", "INT NOT NULL" },
                { "RoomTypeId", "INT NOT NULL" },
                { "MaxCapacity", "INT NOT NULL" }
            };
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
            return RoomId;

        }

        public override string GetPrimaryKeyType()
        {
            return "RoomId";
        }

        public override void SetPrimaryKey(int value)
        {
            RoomId = value;
        }

        public override List<string> GetTableTemplate()
        {
            List<string> template = base.GetTableTemplate();
            template.Add(TableFieldFormat(GetPrimaryKeyType(),Fields[GetPrimaryKeyType()]+" PRIMARY KEY"));
            if (Floor == null)
                Floor = new Floor();
            template.Add(TableFieldFormat(Floor.GetPrimaryKeyType(), Floor.Fields[Floor.GetPrimaryKeyType()]));
            template.Add(TableFieldFormat("Row", Fields["Row"]));
            if (RoomType == null)
                RoomType = new RoomType();
            template.Add(TableFieldFormat(RoomType.GetPrimaryKeyType(),RoomType.Fields[RoomType.GetPrimaryKeyType()]));
            template.Add(TableFieldFormat("MaxCapacity", Fields["MaxCapacity"]));
            return template ;
        }

        public override List<TableData> GetValues()
        {
            List<TableData> values = base.GetValues();
            values.Add(new TableData(GetPrimaryKey().ToString(), GetPrimaryKeyType()));
            values.Add(new TableData(FloorId.ToString(), "FloorId"));
            values.Add(new TableData(Row.ToString(), "Row"));
            values.Add(new TableData(RoomTypeId.ToString(), "RoomTypeId"));
            values.Add(new TableData(MaxCapacity.ToString(), "MaxCapacity"));
            return values;
        }
        public override Dictionary<string, string> GetFields()
        {
            return Fields;
        }

        /// <summary>
        /// Checks if room is available in given time range and capacity
        /// </summary>
        /// <param name="selectedStartTime"></param>
        /// <param name="selectedEndTime"></param>
        /// <param name="numpeople"></param>
        /// <returns></returns>
        public bool IsRoomAvailable(DateTime selectedStartTime, DateTime selectedEndTime,int numpeople)
        {
            if (numpeople > MaxCapacity)
                return false;
            bool isAvailable = true;
            Debug.WriteLine(selectedStartTime + " " + selectedEndTime + " IsAvailableChanged");
            if (selectedStartTime > selectedEndTime)
                return false;
            else
            {
                foreach (var reservation in RoomReservationList.Where(x => x.EndTime >= DateTime.Now))
                {
                    //Check if dates in range
                    //Start before reservation start and end after reservation start
                    //OR Start after reservation start and end after reservation end
                    if (reservation.IsActive)
                    {
                        if ((selectedStartTime <= reservation.StartTime && selectedEndTime >= reservation.StartTime) ||
                            (selectedStartTime >= reservation.StartTime && selectedStartTime <= reservation.EndTime))
                        {
                            isAvailable = false;
                            break;
                        }
                    }

                }
            }
            return isAvailable;
        }

        /// <summary>
        /// Compares rooms by revenue for sort
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Room other)
        {
            decimal thisTotal = 0;
            decimal otherTotal = 0;
            foreach(RoomReservation reservation in RoomReservationList)
            {
                foreach (Transaction transaction in reservation.TransactionList)
                    thisTotal += transaction.ToPayAmount;
            }
            foreach (RoomReservation reservation in other.RoomReservationList)
            {
                foreach (Transaction transaction in reservation.TransactionList)
                    otherTotal += transaction.ToPayAmount;
            }
            if (thisTotal > otherTotal)
                return -1;
            else if (thisTotal < otherTotal)
                return 1;
            else return 0;
        }
    }
}
