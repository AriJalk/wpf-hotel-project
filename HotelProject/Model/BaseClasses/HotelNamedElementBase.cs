using HotelProject.Model.Helpers;
using System.Collections.Generic;
namespace HotelProject.Model.BaseClasses
{
    /// <summary>
    /// Represents an hotel element identified by a unique name
    /// </summary>
    public class HotelNamedElementBase : HotelDbElementBase
    {
        private static Dictionary<string, string> _fields;

        public static new Dictionary<string, string> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }


        private string _name;
        //DB Property
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public HotelNamedElementBase(string name)
        {
            Name = name;
        }

        public HotelNamedElementBase()
        {

        }

        static HotelNamedElementBase()
        {
            Fields = new Dictionary<string, string>(HotelDbElementBase.Fields);
            Fields.Add("Name", "VARCHAR(255) NOT NULL UNIQUE");
        }

        public override List<string> GetTableTemplate()
        {
            List<string> values = base.GetTableTemplate();
            values.Add(TableFieldFormat("Name", Fields["Name"]));
            return values;
        }

        public override List<TableData> GetValues()
        {
            List<TableData> values = base.GetValues();
            values.Add(new TableData(Name, "Name"));
            return values;
        }
        public override Dictionary<string, string> GetFields()
        {
            return Fields;
        }
    }
}
