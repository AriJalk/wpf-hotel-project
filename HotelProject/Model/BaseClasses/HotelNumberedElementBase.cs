using HotelProject.Model.Helpers;
using System.Collections.Generic;

namespace HotelProject.Model.BaseClasses
{
    /// <summary>
    /// Represents an element with unique number
    /// </summary>
    public class HotelNumberedElementBase : HotelDbElementBase
    {
        public static new Dictionary<string, string> Fields { get; set; }

        private int _elementnumber;
        /// <summary>
        /// DB Property
        /// Unique
        /// </summary>
        public int ElementNumber
        {
            get { return _elementnumber; }
            set { _elementnumber = value; }
        }

        public HotelNumberedElementBase(int num)
        {
            ElementNumber = num;
        }

        public HotelNumberedElementBase()
        {

        }

        static HotelNumberedElementBase()
        {
            Fields = new Dictionary<string, string>(HotelDbElementBase.Fields);
            Fields.Add("ElementNumber", "INT NOT NULL UNIQUE");
        }

        public override List<string> GetTableTemplate()
        {
            List<string> values = base.GetTableTemplate();
            values.Add(TableFieldFormat("ElementNumber", Fields["ElementNumber"]));
            return values;
        }

        public override List<TableData> GetValues()
        {
            List<TableData> values = base.GetValues();
            values.Add(new TableData(ElementNumber.ToString(), "ElementNumber"));
            return values;
        }
        public override Dictionary<string, string> GetFields()
        {
            return Fields;
        }
    }
}
