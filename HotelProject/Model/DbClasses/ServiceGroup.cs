using HotelProject.Model.BaseClasses;
using HotelProject.Model.Helpers;
using HotelProject.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject.Model.DbClasses
{
    /// <summary>
    /// Represents possible service groups that services belong to
    /// </summary>
    public class ServiceGroup : HotelNamedElementBase, IIncremented
    {

        public static new Dictionary<string, string> Fields { get; set; }

        static private int _idcount = 0;

        public override int IdCount { get => _idcount; set => _idcount=value; }

        private List<Service> _servicelist;
        /// <summary>
        /// Services belonging to group
        /// </summary>
        public List<Service> ServiceList
        {
            get { return _servicelist; }
            set { _servicelist = value; }
        }

        private int _servicegroupid;
        /// <summary>
        /// Db Property 
        /// Primary Key
        /// </summary>
        public int ServiceGroupId
        {
            get { return _servicegroupid; }
            set 
            {
                _servicegroupid = value;
                if (_servicegroupid > IdCount)
                    IdCount = _servicegroupid;
            }
        }

        /// <summary>
        /// Basic info CTOR
        /// </summary>
        /// <param name="name"></param>
        public ServiceGroup(string name) : base(name)
        {
            ServiceGroupId = ++IdCount;
        }

        public ServiceGroup()
        {
            
        }

        static ServiceGroup()
        {
            Fields = new Dictionary<string, string>(HotelNamedElementBase.Fields);
            Fields.Add("ServiceGroupId", "INT PRIMARY KEY");
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
            return ServiceGroupId;
        }

        public override string GetPrimaryKeyType()
        {
            return "ServiceGroupId";
        }
        public override void SetPrimaryKey(int value)
        {
            ServiceGroupId = value;
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
            if (!string.IsNullOrEmpty(Name))
                IsInDb = true;
        }
    }
}
