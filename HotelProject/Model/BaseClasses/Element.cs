using HotelProject.Model.Helpers;
using HotelProject.Model.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;


namespace HotelProject.Model.BaseClasses
{
    /// <summary>
    /// Represents the most basic functionality of an object used in a database<br/>
    /// must implement
    /// </summary>
    public abstract class Element : ISqlElement
    {
        /// <summary>
        /// Dictionary of all object properties to be added to DB, add values in static constructor
        /// Every DB Property must be listed in the Fields Dictionary for proper reading
        /// Format is "[Field Name]","[Table Args]"
        /// </summary>
        public static Dictionary<string, string> Fields { get; set; }

        static Element()
        {
            Fields = new Dictionary<string, string>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>Field Dictionary</returns>
        public virtual Dictionary<string, string> GetFields()
        {
            return Fields;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Name of field containing primary key</returns>
        public abstract string GetPrimaryKeyType();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Primary key of class</returns>
        public abstract int GetPrimaryKey();
        /// <summary>
        /// Sets primary key
        /// </summary>
        /// <param name="value"></param>
        public abstract void SetPrimaryKey(int value);

        /// <summary>
        /// Helps format the fields to a format
        /// compatible with the table creation methods
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string TableFieldFormat(string type, string args)
        {
            return string.Format("{0} {1}", type, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>All fields formatted to table creation</returns>
        virtual public List<string> GetTableTemplate()
        {
            List<string> template = new List<string>();
            return template;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of fields to add to DB</returns>
        virtual public List<TableData> GetValues()
        {
            List<TableData> values = new List<TableData>();
            return values;
        }

        /// <summary>
        /// Return property by name
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public static object GetProperty(HotelDbElementBase obj, string propertyType)
        {
            if (obj.GetFields()[propertyType] != null)
            {
                var property = obj.GetType().GetProperty(propertyType);
                return property.GetValue(obj);
            }
            return null;
        }

        /// <summary>
        /// Sets property by name
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <param name="source"></param>
        public static void SetProperty(HotelDbElementBase target, string propertyName, object source)
        {
            if (Fields[propertyName] != null)
            {
                var property = target.GetType().GetProperty(propertyName);
                property.SetValue(target, source);
            }
            else
                Debug.WriteLine("Cant set property");
        }
    }
}
