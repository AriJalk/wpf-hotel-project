using HotelProject.Model.BaseClasses;
using HotelProject.Model.Helpers;
using HotelProject.Model.Interfaces;
using System.Collections.Generic;

namespace HotelProject.Model.DbClasses
{
    /// <summary>
    /// Represents a single type of user
    /// </summary>
    public class UserType : HotelNamedElementBase, IIncremented
    {
        public static new Dictionary<string, string> Fields { get; set; }

        private static int _idcount = 0;
        //TODO: Maybe reset method
        public override int IdCount { get => _idcount; set => _idcount = value; }


        private int _usertypeid;
        /// <summary>
        /// DB Property
        /// Primary Key
        /// </summary>
        public int UserTypeId
        {
            get { return _usertypeid; }
            set 
            {
                _usertypeid = value;
                if (_usertypeid > IdCount)
                    IdCount = _usertypeid;
            }
        }


        public UserType(UserTypesEnum type) : base(type.ToString())
        {
            UserTypeId = ++IdCount;
        }

        //TODO: Change Counter
        public UserType()
        {

        }

        static UserType()
        {
            Fields = new Dictionary<string, string>(HotelNamedElementBase.Fields);
            Fields.Add("UserTypeId", "INT NOT NULL");
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
            return UserTypeId;
        }

        public override string GetPrimaryKeyType()
        {
            return "UserTypeId";
        }

        public override void SetPrimaryKey(int value)
        {
            UserTypeId = value;
        }


        public override List<string> GetTableTemplate()
        {
            List<string> template= base.GetTableTemplate();
            template.Add(TableFieldFormat(GetPrimaryKeyType(), Fields[GetPrimaryKeyType()]+" PRIMARY KEY"));
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

        public override string ToString()
        {
            return Name;
        }

        public override void SetInDb()
        {
            if (!string.IsNullOrEmpty(Name))
                IsInDb = true;
        }
    }
}
