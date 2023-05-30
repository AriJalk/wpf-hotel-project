using HotelProject.Model.BaseClasses;
using HotelProject.Model.Helpers;
using HotelProject.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelProject.Model.DbClasses
{
    /// <summary>
    /// Represents Room Type to allow unique policy according to the type<</br>
    /// such as price adjustments
    /// </summary>
    public class RoomType : HotelNamedElementBase, IIncremented
    {

        public static new Dictionary<string, string> Fields { get; set; }

        private static int _idcount = 0;

        public override int IdCount { get => _idcount; set => _idcount = value; }

        private decimal _pricemodifier;
        /// <summary>
        /// Price modifier for transactions, culminative
        /// </summary>
        public decimal PriceModifier
        {
            get { return _pricemodifier; }
            set { _pricemodifier = value; }
        }


        private int _roomtypeid;
        /// <summary>
        /// DB Property Primary Key
        /// </summary>
        public int RoomTypeId
        {
            get { return _roomtypeid; }
            set 
            {
                _roomtypeid = value;
                if (_roomtypeid > IdCount)
                    IdCount = _roomtypeid;
            }
        }

        public RoomType(string name) : base(name) 
        {
            RoomTypeId = ++IdCount;
        }

        /// <summary>
        /// Basic info CTOR
        /// </summary>
        /// <param name="type"></param>
        public RoomType(RoomTypesEnum type, decimal priceModifier) : base(type.ToString())
        {
            PriceModifier = priceModifier;
            RoomTypeId = ++IdCount;
        }

        public RoomType() 
        {
            
        }

        static RoomType()
        {
            Fields = new Dictionary<string, string>(HotelNamedElementBase.Fields);
            Fields.Add("RoomTypeId", "INT NOT NULL");
            Fields.Add("PriceModifier", "CURRENCY NOT NULL");
        }

        public static int GetIdCount()
        {
            return _idcount;
        }

        public static void SetIdCount(int value)
        {
            _idcount = value;
        }


        public override List<string> GetTableTemplate()
        {
            List<string> template = base.GetTableTemplate();
            template.Add(TableFieldFormat(GetPrimaryKeyType(), Fields[GetPrimaryKeyType()]+" PRIMARY KEY"));
            template.Add(TableFieldFormat("PriceModifier", "CURRENCY NOT NULL"));
            return template;
        }

        public override List<TableData> GetValues()
        {
            List<TableData> values = base.GetValues();
            values.Add(new TableData(GetPrimaryKey().ToString(), GetPrimaryKeyType()));
            values.Add(new TableData(PriceModifier.ToString(), "PriceModifier"));
            return values;
        }

        public override int GetPrimaryKey()
        {
            return RoomTypeId;
        }
        public override string GetPrimaryKeyType()
        {
            return "RoomTypeId";
        }

        public override void SetPrimaryKey(int value)
        {
            RoomTypeId = value;
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

        public override string ToString()
        {
            return Name;
        }
    }
}
