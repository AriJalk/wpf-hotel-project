using HotelProject.Model.Helpers;
using HotelProject.Model.Interfaces;
using HotelProject.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace HotelProject.Model.BaseClasses
{
    /// <summary>
    /// Represents the core Hotel Database object functionality which all classes used in the database inherit <br/>
    /// 
    /// Must override Element methods to account for child class unique data<br/>
    /// </summary>
    public class HotelDbElementBase : Element, IIncremented
    {
        public static new Dictionary<string, string> Fields { get; set; }

        private bool _isactive;
        /// <summary>
        /// DB Property, BIT
        /// </summary>
        public bool IsActive
        {
            get { return _isactive; }
            set { _isactive = value; }
        }

        private DateTime _createdtime;
        /// <summary>
        /// DB Property, NOT NULL
        /// </summary>
        public DateTime CreatedTime
        {
            get { return _createdtime; }

            set
            {
                if (value != null)
                {
                    _createdtime = value;
                }
            }
        }

        /// <summary>
        /// Override and set to class static idcount
        /// </summary>
        public virtual int IdCount { get => 0; set { } }

        private bool _isindb;

        public bool IsInDb
        {
            get { return _isindb; }
            set { _isindb = value; }
        }

        public HotelDbElementBase()
        {
            DateTime now = DateTime.Now;
            CreatedTime = now;
            IsActive = true;
            IsInDb = false;
        }

        public HotelDbElementBase(bool read) { }



        static HotelDbElementBase()
        {
            Fields = new Dictionary<string, string>(Element.Fields);
            Fields.Add("IsActive", "BIT NOT NULL");
            Fields.Add("CreatedTime", "DATETIME NOT NULL");
        }

        public virtual List<string> GenerateErrors()
        {
            List<string> errors = new List<string>();
            if (CreatedTime == null)
                errors.Add("CreatedTime");
            return errors;
        }

        public bool ValidateData()
        {
            List<string> errors = this.GenerateErrors();
            if (errors.Count > 0)
            {
                string message = string.Empty;
                foreach (string error in errors)
                    message += string.Format("{0} Not Valid\n", error);
                MessageBox.Show(message);
                return false;
            }
            Debug.WriteLine("Object Validation = SUCCESS");
            return true;
        }

        public override List<string> GetTableTemplate()
        {
            List<string> template = base.GetTableTemplate();
            template.Add(TableFieldFormat("IsActive", Fields["IsActive"]));
            template.Add(TableFieldFormat("CreatedTime", Fields["CreatedTime"]));
            return template;
        }

        public override List<TableData> GetValues()
        {
            List<TableData> values = base.GetValues();
            values.Add(new TableData(Converters.BoolToTable(IsActive), "IsActive"));
            values.Add(new TableData(CreatedTime.ToString(), "CreatedTime"));
            return values;
        }

        public override string GetPrimaryKeyType()
        {
            return string.Empty;
        }

        public override int GetPrimaryKey()
        {
            return 0;
        }

        public override void SetPrimaryKey(int value) { }

        public override Dictionary<string, string> GetFields()
        {
            return Fields;
        }

        /// <summary>
        /// Sets a single object into an object
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="propertyType"></param>
        public void SetObjectProperty(HotelDbElementBase target, HotelDbElementBase source, string propertyType)
        {
            //targetType and property used as accessors to the object field by name
            var targetType = target.GetType();
            var property = targetType.GetProperty(propertyType);
            if (property != null && target != null && source != null)
            {
                property.SetValue(target, source);
                Debug.WriteLine($"Set Object Property {propertyType}: SUCCESS");
            }
            else
                Debug.WriteLine($"Set Object Property {propertyType}: FAIL");
        }

        /// <summary>
        /// Sets a list into target object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="propertyType"></param>
        public void SetObjectList<T>(HotelDbElementBase target, List<T> source, string propertyType)
        {
            //targetType and property used as accessors to the object field by name
            var targetType = target.GetType();
            var property = targetType.GetProperty(propertyType);
            if (property != null)
            {
                property.SetValue(target, source);
                Debug.WriteLine($"Set Object List {propertyType}: SUCCESS");
            }
            else
                Debug.WriteLine($"Set Object List {propertyType}: FAIL");
        }

        /// <summary>
        /// Receives object and field name and returns property if exist
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyType"></param>
        /// <returns>Object property</returns>
        public object GetObjectProperty(HotelDbElementBase obj, string propertyType)
        {
            var targetType = obj.GetType();
            var property = targetType.GetProperty(propertyType);
            if (property != null)
            {
                Debug.WriteLine($"Get Object Property {propertyType} - {property.Name}: SUCCESS");
                return property.GetValue(obj);
            }

            Debug.WriteLine($"Get Object Property {propertyType} - {property.Name}: FAIL");
            return null;


        }

        /// <summary>
        /// Sets DB state to true if 
        /// required parameters exist
        /// </summary>
        public virtual void SetInDb()
        {
            IsInDb = true;
        }

        public virtual void DeactivateItem()
        {
            IsActive = false;
        }
    }
}
