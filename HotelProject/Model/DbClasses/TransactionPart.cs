using HotelProject.Model.BaseClasses;
using HotelProject.Model.Helpers;
using HotelProject.Model.Interfaces;
using System.Collections.Generic;

namespace HotelProject.Model.DbClasses
{
    /// <summary>
    /// Represents a single transaction part correlating to a service
    /// </summary>
    public class TransactionPart : HotelDbElementBase, IIncremented
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

        private int _transactionpartid;
        /// <summary>
        /// DB Property
        /// Primary Key
        /// </summary>
        public int TransactionPartId
        {
            get { return _transactionpartid; }
            set
            {
                _transactionpartid = value;
                if (_transactionpartid > IdCount)
                    IdCount = _transactionpartid;
            }
        }

        private int _transactionid;
        /// <summary>
        /// DB Property
        /// </summary>
        public int TransactionId
        {
            get { return _transactionid; }
            set { _transactionid = value; }
        }


        private Transaction _transaction;
        /// <summary>
        /// The transaction the part belong to
        /// </summary>
        public Transaction Transaction
        {
            get
            {
                return _transaction;
            }
            set
            {
                if (value != null)
                {
                    _transaction = value;
                    TransactionId = _transaction.GetPrimaryKey();
                }
            }
        }

        private int _serviceid;
        /// <summary>
        /// DB Property
        /// ServiceId
        /// </summary>
        public int ServiceId
        {
            get { return _serviceid; }
            set { _serviceid = value; }
        }


        private Service _service;
        /// <summary>
        /// The service the part represents
        /// </summary>
        public Service Service
        {
            get { return _service; }
            set
            {
                if (value != null)
                {
                    _service = value;
                    ServiceId = _service.ServiceId;
                }

            }
        }

        private decimal _Price;
        /// <summary>
        /// DB Property
        /// </summary>
        public decimal Price
        {
            get { return _Price; }
            set
            {
                if (Transaction != null)
                    Transaction.ToPayAmount -= _Price;
                _Price = value;
                if (Transaction != null)
                    Transaction.ToPayAmount += _Price;
            }
        }

        private int _referenceid;
        /// <summary>
        /// DB Property is container exists
        /// </summary>
        public int ReferenceId
        {
            get { return _referenceid; }
            set { _referenceid = value; }
        }

        //TODO: remove
        private HotelDbElementBase _container;
        /// <summary>
        /// Contains reference object (usually of type RoomReservation)
        /// </summary>
        public HotelDbElementBase Container
        {
            get
            {
                return _container;
            }
            set
            {
                _container = value;
                ReferenceId = _container.GetPrimaryKey();
            }
        }

        public TransactionPart()
        {

        }

        public TransactionPart(Transaction transaction, Service service, RoomType roomType, int count)
        {
            _transactionpartid = IdCount + 1;
            Transaction = transaction;
            Service = service;
            Price = (service.Price + roomType.PriceModifier) * count;
            ReferenceId = 0;
        }

        public TransactionPart(Transaction transaction, Service service, decimal price)
        {
            _transactionpartid = IdCount + 1;
            Transaction = transaction;
            Service = service;
            Price = price;
            ReferenceId = 0;
        }


        static TransactionPart()
        {
            Fields = new Dictionary<string, string>(HotelDbElementBase.Fields);
            Fields.Add("TransactionPartId", "INT NOT NULL PRIMARY KEY");
            Fields.Add("TransactionId", "INT NOT NULL");
            Fields.Add("ServiceId", "INT NOT NULL");
            Fields.Add("Price", "CURRENCY NOT NULL");
            Fields.Add("ReferenceId", "INT");
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
            return TransactionPartId;
        }

        public override string GetPrimaryKeyType()
        {
            return "TransactionPartId";
        }

        public override void SetPrimaryKey(int value)
        {
            TransactionPartId = value;
        }


        public override List<string> GetTableTemplate()
        {
            List<string> template = base.GetTableTemplate();
            template.Add(TableFieldFormat(GetPrimaryKeyType(), Fields[GetPrimaryKeyType()]));
            template.Add(TableFieldFormat("TransactionId", Fields["TransactionId"]));
            template.Add(TableFieldFormat("ServiceId", Fields["ServiceId"]));
            template.Add(TableFieldFormat("Price", Fields["Price"]));
            template.Add(TableFieldFormat("ReferenceId", Fields["ReferenceId"]));
            return template;

        }

        public override List<TableData> GetValues()
        {
            List<TableData> values = base.GetValues();
            values.Add(new TableData(GetPrimaryKey().ToString(), GetPrimaryKeyType()));
            values.Add(new TableData(Transaction.GetPrimaryKey().ToString(), Transaction.GetPrimaryKeyType()));
            values.Add(new TableData(ServiceId.ToString(), "ServiceId"));
            values.Add(new TableData(Price.ToString(), "Price"));
            values.Add(new TableData(ReferenceId.ToString(), "ReferenceId"));
            return values;
        }

        public override Dictionary<string, string> GetFields()
        {
            return Fields;
        }
        public void SetTempId(int id)
        {
            _transactionpartid = id;
        }

        public override void SetInDb()
        {
            if (TransactionId > 0)
                IsInDb = true;
        }
    }
}
