using HotelProject.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelProject.Model.BaseClasses
{
    /// <summary>
    /// Represents a person, inherited by user and customer
    /// </summary>
    public class Person : HotelDbElementBase
    {
        public static new Dictionary<string, string> Fields { get; set; }

        private string _fname;
        /// <summary>
        /// DB Property
        /// First Name
        /// </summary>
        public string FName
        {
            get { return _fname; }
            set { _fname = value; }
        }

        private string _lname;
        /// <summary>
        /// DB Property
        /// Last Name
        /// </summary>
        public string LName
        {
            get { return _lname; }
            set { _lname = value; }
        }

        private string _phonenumber;
        /// <summary>
        /// DB Property
        /// Phone Number
        /// </summary>
        public string PhoneNumber
        {
            get { return _phonenumber; }
            set { _phonenumber = value; }
        }


        private string _idnumber;
        /// <summary>
        /// DB Property
        /// I.D Number
        /// </summary>
        public string IdNumber
        {
            get { return _idnumber; }
            set 
            { 
                _idnumber = value;
            }
        }

        /// <summary>
        /// Full Parameter CTOR
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="lname"></param>
        /// <param name="phonenumber"></param>
        /// <param name="idnumber"></param>
        public Person(string fname, string lname, string phonenumber, string idnumber)
        {
            FName = fname;
            LName = lname;
            PhoneNumber = phonenumber;
            IdNumber = idnumber;
        }

        public Person()
        {

        }

        static Person()
        {
            Fields = new Dictionary<string, string>(HotelDbElementBase.Fields);
            Fields.Add("FName", "VARCHAR(50) NOT NULL");
            Fields.Add("LName", "VARCHAR(50) NOT NULL");
            Fields.Add("PhoneNumber", "VARCHAR(20) NOT NULL UNIQUE");
            Fields.Add("IdNumber", "VARCHAR(20) NOT NULL UNIQUE");
        }

        public override List<string> GetTableTemplate()
        {
            List<string> template = base.GetTableTemplate();
            template.Add(TableFieldFormat("FName",Fields["FName"]));
            template.Add(TableFieldFormat("LName",Fields["LName"]));
            template.Add(TableFieldFormat("PhoneNumber",Fields["PhoneNumber"]));
            template.Add(TableFieldFormat("IdNumber", Fields["IdNumber"]));
            return template;
        }

        public override List<TableData> GetValues()
        {
            List<TableData> values = base.GetValues();
            values.Add(new TableData(FName, "FName"));
            values.Add(new TableData(LName, "LName"));
            values.Add(new TableData(PhoneNumber, "PhoneNumber"));
            values.Add(new TableData(IdNumber, "IdNumber"));
            return values;
        }
        public override Dictionary<string, string> GetFields()
        {
            return Fields;
        }

        public override List<string> GenerateErrors()
        {
            List<string> errors = base.GenerateErrors();
            if (string.IsNullOrEmpty(FName))
                errors.Add("First Name");
            if (string.IsNullOrEmpty(LName))
                errors.Add("Last Name");
            if (string.IsNullOrEmpty(PhoneNumber))
                errors.Add("Phone Number");
            else
            {
                if (PhoneNumber.Length > 12)
                    errors.Add("Phone Number");
                else
                {
                    long number;
                    bool success = long.TryParse(PhoneNumber, out number);
                    if (!success)
                        errors.Add("Phone Number");
                }
            }
            if (string.IsNullOrEmpty(IdNumber))
                errors.Add("I.D Number");
            return errors;
        }
    }
}
