using HotelProject.Model.BaseClasses;
using HotelProject.Model.Helpers;
using HotelProject.Model.Interfaces;
using HotelProject.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject.Model.DbClasses
{
    /// <summary>
    /// Represents a single transaction made for a reservation
    /// Each transaction can be made up of multiple parts
    /// </summary>
    public class Transaction : HotelDbElementBase, IIncremented
    {
        public static new Dictionary<string, string> Fields { get; set; }


        private static int _idcount = 0;

        public override int IdCount { get => _idcount; set => _idcount = value; }

        private int _transactionid;
        /// <summary>
        /// DB Property
        /// Primary Key
        /// </summary>
        public int TransactionId
        {
            get { return _transactionid; }
            set
            {
                _transactionid = value;
                if (_transactionid > IdCount)
                    IdCount = _transactionid;

            }
        }

        private int _userid;
        //DB Property
        public int UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }

        private int _roomreservationid;
        /// <summary>
        /// DB Property
        /// </summary>
        public int RoomReservationId
        {
            get { return _roomreservationid; }
            set { _roomreservationid = value; }
        }

        private RoomReservation _roomreservation;
        /// <summary>
        /// The linked reservation
        /// </summary>
        public RoomReservation RoomReservation
        {
            get
            {
                return _roomreservation;
            }
            set
            {
                _roomreservation = value;
                RoomReservationId = _roomreservation.GetPrimaryKey();
                Customer = _roomreservation.Customer;
            }
        }


        private List<TransactionPart> _transactionpartlist;
        /// <summary>
        /// List of transaction parts belonging to a single transaction
        /// </summary>
        public List<TransactionPart> TransactionPartList
        {
            get { return _transactionpartlist; }
            set
            {
                _transactionpartlist = value;
                if (_transactionpartlist != null && _transactionpartlist.Count > 0)
                {
                    ToPayAmount = 0;
                    foreach (TransactionPart part in _transactionpartlist)
                        ToPayAmount += part.Price;
                }
            }
        }

        private string _credittransaction;
        /// <summary>
        /// DB Property
        /// </summary>
        public string CreditTransaction
        {
            get { return _credittransaction; }
            set { _credittransaction = value; }
        }

        private bool _isvalidated;
        /// <summary>
        /// DB Property
        /// </summary>
        public bool IsValidated
        {
            get { return _isvalidated; }
            set { _isvalidated = value; }
        }

        private bool _ispayed;
        /// <summary>
        /// DB Property
        /// </summary>
        public bool IsPayed
        {
            get { return _ispayed; }
            set { _ispayed = value; }
        }

        private bool _isrefunded;
        /// <summary>
        /// DB Propertry
        /// Cancalation of transaction/reservation
        /// </summary>
        public bool IsRefunded
        {
            get { return _isrefunded; }
            set
            {
                _isrefunded = value;
            }
        }

        private User _user;

        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                _userid = _user.GetPrimaryKey();
            }
        }

        private decimal _topayamount;
        /// <summary>
        /// DB Property 
        /// Currency
        /// </summary>
        public decimal ToPayAmount
        {
            get { return _topayamount; }
            set { _topayamount = value; }
        }

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        private string _servicetype;

        public string ServiceType
        {
            get { return _servicetype; }
            set { _servicetype = value; }
        }



        //Copy constructor
        public Transaction(Transaction source)
        {
            this.CreatedTime = source.CreatedTime;
            this.CreditTransaction = source.CreditTransaction;
            this.IdCount = source.IdCount;
            this.IsActive = source.IsActive;
            this.IsInDb = source.IsInDb;
            this.IsPayed = source.IsPayed;
            this.IsRefunded = source.IsRefunded;
            this.IsValidated = source.IsValidated;
            this.RoomReservation = new RoomReservation(source.RoomReservation);
            this.ToPayAmount = source.ToPayAmount;
            this.TransactionId = source.TransactionId;
            this.TransactionPartList = source.TransactionPartList;
            this.User = source.User;
        }

        public Transaction()
        {

        }

        public Transaction(RoomReservation reservation, User user, List<TransactionPart> parts, string type)
        {
            ToPayAmount = 0;
            _transactionid = IdCount + 1;
            RoomReservation = reservation;
            User = user;
            TransactionPartList = parts;
            ServiceType = type;
            foreach (TransactionPart part in parts)
                ToPayAmount += part.Price;
        }

        public Transaction(RoomReservation reservation, User user, string type)
        {
            ToPayAmount = 0;
            _transactionid = IdCount + 1;
            RoomReservation = reservation;
            User = user;
            ServiceType = type;
        }


        static Transaction()
        {
            Fields = new Dictionary<string, string>(HotelDbElementBase.Fields)
            {
                { "TransactionId", "INT NOT NULL PRIMARY KEY" },
                { "ServiceType", "VARCHAR(255) NOT NULL" },
                { "RoomReservationId", "INT NOT NULL" },
                { "IsValidated", "BIT NOT NULL" },
                { "IsPayed", "BIT NOT NULL" },
                { "IsRefunded", "BIT NOT NULL" },
                { "UserId", "INT NOT NULL" },
                {"ToPayAmount","CURRENCY NOT NULL" }
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
            return TransactionId;
        }

        public override string GetPrimaryKeyType()
        {
            return "TransactionId";
        }

        public override void SetPrimaryKey(int value)
        {
            TransactionId = value;
        }


        public override List<string> GetTableTemplate()
        {
            List<string> template = base.GetTableTemplate();
            template.Add(TableFieldFormat(GetPrimaryKeyType(), Fields[GetPrimaryKeyType()]));
            template.Add(TableFieldFormat("ServiceType", Fields["ServiceType"]));
            template.Add(TableFieldFormat("RoomReservationId", Fields["RoomReservationId"]));
            template.Add(TableFieldFormat("ToPayAmount", Fields["ToPayAmount"]));
            template.Add(TableFieldFormat("IsValidated", Fields["IsValidated"]));
            template.Add(TableFieldFormat("IsPayed", Fields["IsPayed"]));
            template.Add(TableFieldFormat("IsRefunded", Fields["IsRefunded"]));
            template.Add(TableFieldFormat("UserId", Fields["UserId"]));
            return template;
        }

        public override List<TableData> GetValues()
        {
            List<TableData> values = base.GetValues();
            values.Add(new TableData(GetPrimaryKey().ToString(), GetPrimaryKeyType()));
            values.Add(new TableData(ServiceType, "ServiceType"));
            values.Add(new TableData(RoomReservationId.ToString(), "RoomReservationId"));
            values.Add(new TableData(ToPayAmount.ToString(), "ToPayAmount"));
            values.Add(new TableData(Converters.BoolToTable(IsValidated), "IsValidated"));
            values.Add(new TableData(Converters.BoolToTable(IsPayed), "IsPayed"));
            values.Add(new TableData(Converters.BoolToTable(IsRefunded), "IsRefunded"));
            values.Add(new TableData(UserId.ToString(), "UserId"));
            return values;
        }
        public override Dictionary<string, string> GetFields()
        {
            return Fields;
        }

        public override List<string> GenerateErrors()
        {
            List<string> errors = base.GenerateErrors();
            if (RoomReservationId == 0)
                errors.Add("Reservation not valid");
            if (UserId == 0)
                errors.Add("User Not Valid");
            if (ToPayAmount < 0)
                errors.Add("To Pay amount not Valid");
            return errors;
        }

        /// <summary>
        /// Method to add parts to transaction
        /// </summary>
        /// <param name="part"></param>
        public void AddToPartList(TransactionPart part)
        {
            TransactionPartList.Add(part);
            ToPayAmount += part.Price;
        }

        public override void SetInDb()
        {
            if (RoomReservationId > 0)
                IsInDb = true;
        }

        /// <summary>
        /// Refund procedure
        /// cancel all parts and linked reservation
        /// </summary>
        public void Refund()
        {
            IsRefunded = true;
            ToPayAmount = 0;
            if (ServiceType == "Lodging")
            {
                if (RoomReservation != null)
                    RoomReservation.IsActive = false;
                foreach (TransactionPart part in TransactionPartList)
                {
                    part.IsActive = false;
                }
            }
        }

        /// <summary>
        /// Archive transaction if conditions apply (CheckIn+CheckOut and enough days passed)
        /// </summary>
        public void Archive()
        {
            int daysPolicy = 30;
            bool refund=RoomReservation.TransactionList[0].IsRefunded;
            TimeSpan ts = DateTime.Now - RoomReservation.EndTime;
            
            if(ts.Days>=daysPolicy&&refund)
            {
                foreach (TransactionPart part in TransactionPartList)
                {
                    if (SqlDatabaseHelper.InsertArchive(part))
                        SqlDatabaseHelper.Delete(part);
                }
                if (SqlDatabaseHelper.InsertArchive(this))
                    SqlDatabaseHelper.Delete(this);
            }
            else if(ts.Days>=daysPolicy&&this.RoomReservation.IsCheckIn&&this.RoomReservation.IsCheckOut)
            {
                foreach(TransactionPart part in TransactionPartList)
                {
                    if(SqlDatabaseHelper.InsertArchive(part))
                        SqlDatabaseHelper.Delete(part);
                }
                if(SqlDatabaseHelper.InsertArchive(this))
                    SqlDatabaseHelper.Delete(this);
            }
        }
    }
}
