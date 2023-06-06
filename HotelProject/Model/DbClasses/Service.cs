using HotelProject.Model.BaseClasses;
using HotelProject.Model.Helpers;
using HotelProject.Model.Interfaces;
using System.Collections.Generic;

namespace HotelProject.Model.DbClasses
{
    /// <summary>
    /// Represents a global paid service for transaction parts
    /// </summary>
    public class Service : HotelNamedElementBase, IIncremented
    {
        public static new Dictionary<string, string> Fields { get; set; }

        public static int _idcount = 0;
        public override int IdCount { get => _idcount; set => _idcount = value; }

        private int _serviceid;
        /// <summary>
        /// DB Property
        /// Primary Key
        /// </summary>
        public int ServiceId
        {
            get { return _serviceid; }
            set 
            {
                _serviceid = value;
                if (_serviceid > IdCount)
                    IdCount = _serviceid;
            }
        }

        private int _servicegroupid;
        /// <summary>
        /// DB Property
        /// </summary>
        public int ServiceGroupId
        {
            get { return _servicegroupid; }
            set { _servicegroupid = value; }
        }


        private ServiceGroup _servicegroup;
        /// <summary>
        /// The service group the object belongs to
        /// </summary>
        public ServiceGroup ServiceGroup
        {
            get 
            { 
                return _servicegroup;
            }
            set
            { 
                _servicegroup = value;
                ServiceGroupId = _servicegroup.GetPrimaryKey();
            }
        }

        private decimal _price;
        /// <summary>
        /// DB Property 
        /// Currency
        /// </summary>
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        /// <summary>
        /// Basic info CTOR
        /// </summary>
        /// <param name="name"></param>
        /// <param name="group"></param>
        public Service(string name, ServiceGroup group, decimal price) : base(name)
        {
            ServiceGroup = group;
            ServiceId = ++IdCount;
            Price = price;
        }

        public Service()
        {
            
        }

        static Service()
        {
            Fields = new Dictionary<string, string>(HotelNamedElementBase.Fields);
            Fields.Add("ServiceId", "INT NOT NULL");
            Fields.Add("ServiceGroupId", "INT NOT NULL");
            Fields.Add("Price", "CURRENCY NOT NULL");
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
            return ServiceId;
        }

        public override string GetPrimaryKeyType()
        {
            return "ServiceId";
        }

        public override void SetPrimaryKey(int value)
        {
            ServiceId = value;
        }

        public override List<string> GetTableTemplate()
        {
            List<string> template = base.GetTableTemplate();
            template.Add(TableFieldFormat(GetPrimaryKeyType(), Fields[GetPrimaryKeyType()] + " PRIMARY KEY"));
            if (ServiceGroup == null)
                ServiceGroup = new ServiceGroup();
            template.Add(TableFieldFormat(ServiceGroup.GetPrimaryKeyType().ToString(), Fields[ServiceGroup.GetPrimaryKeyType()]));
            template.Add(TableFieldFormat("Price", Fields["Price"]));
            return template;
        }

        public override List<TableData> GetValues()
        {
            List<TableData> values = base.GetValues();
            values.Add(new TableData(GetPrimaryKey().ToString(), GetPrimaryKeyType()));
            values.Add(new TableData(ServiceGroup.GetPrimaryKey().ToString(), ServiceGroup.GetPrimaryKeyType().ToString()));
            values.Add(new TableData(Price.ToString(), "Price"));
            return values;
        }
        public override Dictionary<string, string> GetFields()
        {
            return Fields;
        }

        public override void SetInDb()
        {
            if (!string.IsNullOrEmpty(Name))
                IsInDb = true;
        }
    }
}
