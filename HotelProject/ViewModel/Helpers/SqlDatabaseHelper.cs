using HotelProject.Model;
using HotelProject.Model.BaseClasses;
using HotelProject.Model.DbClasses;
using HotelProject.Model.Helpers;
using HotelProject.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelProject.ViewModel.Helpers
{
    /// <summary>
    /// Handles all communication with the sql database<br/>
    /// Table naming is plural of [class]
    /// </summary>
    public static class SqlDatabaseHelper
    {
        public enum Operation
        {
            Read,
            Insert,
            Update,
            Delete,
            InnerJoin,
            CreateTable
        }

        private static readonly string dbFile = Path.Combine(Environment.CurrentDirectory, "DB.accdb");
        private static readonly string connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Jet OLEDB:Database Password=password;", dbFile);


        private static string FormatInsertValues(HotelDbElementBase obj)
        {
            string values = string.Empty;
            List<TableData> list = obj.GetValues();
            foreach (var entry in list)
            {
                values += string.Format("'{0}', ", entry.Field);
            }
            return Trim(values);
        }
        private static string FormatUpdateValues(HotelDbElementBase obj)
        {
            string values = string.Empty;
            List<TableData> list = obj.GetValues();
            foreach (var entry in list)
            {
                values += string.Format("{0}= '{1}', ", entry.FieldName, entry.Field);
            }
            return Trim(values);
        }
        private static string FormatTableValues(HotelDbElementBase obj)
        {
            string values = string.Empty;
            List<string> list = obj.GetTableTemplate();
            foreach (var index in list)
            {
                values += string.Format("{0}, ", index);
            }
            return Trim(values);
        }
        /// <summary>
        /// Creates Sql Command
        /// Receives requires operation and object to create sql command from
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        private static string CreateCommand(HotelDbElementBase obj, Operation op)
        {
            var classname = obj.GetType().Name;
            if (op == Operation.Insert)
                return string.Format("INSERT INTO {0}Table VALUES ({1});",
                   classname, FormatInsertValues(obj));

            if (op == Operation.Delete)
                return string.Format("DELETE FROM {0}Table WHERE {1}={2};",
                    classname, obj.GetPrimaryKeyType(), obj.GetPrimaryKey());
            if (op == Operation.Update)
                return string.Format("UPDATE {0}Table SET {1} WHERE {2}={3};",
                    classname, FormatUpdateValues(obj), obj.GetPrimaryKeyType(), obj.GetPrimaryKey());
            else return "";
        }

        private static string CreateCommandArchive(HotelDbElementBase obj, Operation op)
        {
            var classname = obj.GetType().Name;
            if (op == Operation.Insert)
                return string.Format("INSERT INTO {0}Table_archive VALUES ({1});",
                   classname, FormatInsertValues(obj));

            if (op == Operation.Delete)
                return string.Format("DELETE FROM {0}Table_archive WHERE {1}='{2}';",
                    classname, obj.GetPrimaryKeyType(), obj.GetPrimaryKey());
            if (op == Operation.Update)
                return string.Format("UPDATE {0}Table_archive SET {1} WHERE {2}={3};",
                    classname, FormatUpdateValues(obj), obj.GetPrimaryKeyType(), obj.GetPrimaryKey());
            else return "";
        }
        private static string Trim(string str)
        {
            return str.TrimEnd(',', ' ');
        }

        /// <summary>
        /// Create table method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool CreateTable<T>() where T : HotelDbElementBase, new()
        {
            T t_access = new T();
            bool result = false;
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = conn;
                    command.CommandText = string.Format("CREATE TABLE {0}Table ({1});",
                        t_access.GetType().Name, FormatTableValues(t_access));
                    Debug.WriteLine("Command Text: " + command.CommandText);
                    command.ExecuteNonQuery();
                    conn.Close();
                    result = true;
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("Exception: " + err.Message);
                return false;
            }
            return result;

        }

        public static bool DeleteTable<T>() where T : HotelDbElementBase, new()
        {
            T t_access = new T();
            bool result = false;
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = conn;
                    command.CommandText = string.Format("DROP TABLE {0}Table;",
                        t_access.GetType().Name);
                    Debug.WriteLine("Command Text: " + command.CommandText);
                    command.ExecuteNonQuery();
                    conn.Close();
                    result = true;
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("Exception: " + err.Message);
                return false;
            }
            return result;

        }

        /// <summary>
        /// Non query data method
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        private static bool WriteDB(HotelDbElementBase obj, Operation op)
        {
            bool result = false;
            OleDbCommand command = new OleDbCommand(CreateCommand(obj, op));
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    command.Connection = conn;
                    conn.Open();
                    Debug.WriteLine("Command Text: " + command.CommandText);
                    command.ExecuteNonQuery();
                    conn.Close();
                    result = true;
                    obj.IsInDb = true;
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("Exception: " + err.Message);
                return false;
            }
            return result;
        }

        /// <summary>
        /// Non query data method - Archive mode
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        private static bool WriteDBArchive(HotelDbElementBase obj, Operation op)
        {
            bool result = false;
            OleDbCommand command = new OleDbCommand(CreateCommandArchive(obj, op));
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    command.Connection = conn;
                    conn.Open();
                    Debug.WriteLine("Command Text: " + command.CommandText);
                    command.ExecuteNonQuery();
                    conn.Close();
                    result = true;
                    obj.IsInDb = true;
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("Exception: " + err.Message);
                return false;
            }
            return result;
        }

        /// <summary>
        /// Basic Reading Operation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> Read<T>() where T : HotelDbElementBase, new()
        {
            List<T> list = new List<T>();
            var t_access = new T();
            t_access.SetPrimaryKey(0);
            t_access.IdCount = 0;
            int index = 0;
            var fields = t_access.GetFields();
            var objtype_access = t_access.GetType();
            OleDbCommand command = new OleDbCommand(string.Format("SELECT * FROM {0}Table;",
                t_access.GetType().Name));
            int maxid = 0;
            //Open connection to DB
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    command.Connection = conn;
                    conn.Open();
                    OleDbDataReader dr;
                    Debug.WriteLine("Command Text: " + command.CommandText);
                    //Execture query
                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        list.Add(new T());
                        /*Every DB property corresponds to a mapped entry in the class static dictionary
                        Since dictionary order is not controllable, read every key corresponding
                        to a field from every dictionary pair and assign values to the property 
                        corresponding to the key name*/
                        foreach (var pair in fields)
                        {
                            var property = objtype_access.GetProperty(pair.Key);
                            property.SetValue(list[index], dr[pair.Key]);
                        }
                        list[index].IsInDb = true;
                        if (list[index].GetPrimaryKey() > maxid)
                            maxid = list[index].GetPrimaryKey();
                        index++;
                    }
                    dr.Close();
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("Exception: " + err.Message);
            }
            t_access.IdCount = maxid;
            return list;
        }

        /// <summary>
        /// Basic Reading Operation - Archive mode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> ReadArchive<T>() where T : HotelDbElementBase, new()
        {
            List<T> list = new List<T>();
            var t_access = new T();
            t_access.SetPrimaryKey(0);
            t_access.IdCount = 0;
            int index = 0;
            var fields = t_access.GetFields();
            var objtype_access = t_access.GetType();
            OleDbCommand command = new OleDbCommand(string.Format("SELECT * FROM {0}Table_archive;",
                t_access.GetType().Name));
            int maxid = 0;
            //Open connection to DB
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    command.Connection = conn;
                    conn.Open();
                    OleDbDataReader dr;
                    Debug.WriteLine("Command Text: " + command.CommandText);
                    //Execture query
                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        list.Add(new T());
                        /*Every DB property corresponds to a mapped entry in the class static dictionary
                        Since dictionary order is not controllable, read every key corresponding
                        to a field from every dictionary pair and assign values to the property 
                        corresponding to the key name*/
                        foreach (var pair in fields)
                        {
                            var property = objtype_access.GetProperty(pair.Key);
                            property.SetValue(list[index], dr[pair.Key]);
                        }
                        list[index].IsInDb = true;
                        if (list[index].GetPrimaryKey() > maxid)
                            maxid = list[index].GetPrimaryKey();
                        index++;
                    }
                    dr.Close();
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("Exception: " + err.Message);
            }
            t_access.IdCount = maxid;
            return list;
        }

        /// <summary>
        /// Specified Basic reading
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="where_args"></param>
        /// <returns></returns>
        public static List<T> Read<T>(string where_args) where T : HotelDbElementBase, new()
        {
            List<T> list = new List<T>();
            var t_access = new T();
            t_access.SetPrimaryKey(0);
            t_access.IdCount = 0;
            int index = 0;
            var fields = t_access.GetFields();
            var objtype_access = t_access.GetType();
            OleDbCommand command = new OleDbCommand(string.Format("SELECT * FROM {0}Table WHERE {1};",
                t_access.GetType().Name, where_args));
            int maxid = 0;
            //Open connection to DB
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    command.Connection = conn;
                    conn.Open();
                    OleDbDataReader dr;
                    Debug.WriteLine("Command Text: " + command.CommandText);
                    //Execture query
                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        list.Add(new T());
                        /*Every DB property corresponds to a mapped entry in the class static dictionary
                        Since dictionary order is not controllable, read every key corresponding
                        to a field from every dictionary pair and assign values to the property 
                        corresponding to the key name*/
                        foreach (var pair in fields)
                        {
                            var property = objtype_access.GetProperty(pair.Key);
                            property.SetValue(list[index], dr[pair.Key]);
                        }
                        list[index].IsInDb = true;
                        if (list[index].GetPrimaryKey() > maxid)
                            maxid = list[index].GetPrimaryKey();
                        index++;
                    }
                    dr.Close();
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("Exception: " + err.Message);
            }
            t_access.IdCount = maxid;
            return list;
        }

        /// <summary>
        /// Specified Basic reading - Archive mode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="where_args"></param>
        /// <returns></returns>
        public static List<T> ReadArchive<T>(string where_args) where T : HotelDbElementBase, new()
        {
            List<T> list = new List<T>();
            var t_access = new T();
            t_access.SetPrimaryKey(0);
            t_access.IdCount = 0;
            int index = 0;
            var fields = t_access.GetFields();
            var objtype_access = t_access.GetType();
            OleDbCommand command = new OleDbCommand(string.Format("SELECT * FROM {0}Table_archive WHERE {1};",
                t_access.GetType().Name, where_args));
            int maxid = 0;
            //Open connection to DB
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    command.Connection = conn;
                    conn.Open();
                    OleDbDataReader dr;
                    Debug.WriteLine("Command Text: " + command.CommandText);
                    //Execture query
                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        list.Add(new T());
                        /*Every DB property corresponds to a mapped entry in the class static dictionary
                        Since dictionary order is not controllable, read every key corresponding
                        to a field from every dictionary pair and assign values to the property 
                        corresponding to the key name*/
                        foreach (var pair in fields)
                        {
                            var property = objtype_access.GetProperty(pair.Key);
                            property.SetValue(list[index], dr[pair.Key]);
                        }
                        list[index].IsInDb = true;
                        if (list[index].GetPrimaryKey() > maxid)
                            maxid = list[index].GetPrimaryKey();
                        index++;
                    }
                    dr.Close();
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("Exception: " + err.Message);
            }
            t_access.IdCount = maxid;
            return list;
        }

        public static T ReadSingle<T>(string where_args) where T : HotelDbElementBase, new()
        {
            T singleObject = new T();
            var t_access = new T();
            t_access.SetPrimaryKey(0);
            t_access.IdCount = 0;
            var fields = t_access.GetFields();
            var objtype_access = t_access.GetType();
            OleDbCommand command = new OleDbCommand(string.Format("SELECT * FROM {0}Table WHERE {1};",
                t_access.GetType().Name, where_args));
            //Open connection to DB
            try
            {
                using (var conn = new OleDbConnection(connectionString))
                {
                    command.Connection = conn;
                    conn.Open();
                    OleDbDataReader dr;
                    Debug.WriteLine("Command Text: " + command.CommandText);
                    //Execture query
                    dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        foreach (var pair in fields)
                        {
                            var property = objtype_access.GetProperty(pair.Key);
                            property.SetValue(singleObject, dr[pair.Key]);
                        }
                    }
                    dr.Close();
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("Exception: " + err.Message);
            }
            singleObject.SetInDb();
            return singleObject;
        }

        /// <summary>
        /// Inserts or updates object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Insert(HotelDbElementBase obj)
        {
            if (obj.IsInDb == false)
                return WriteDB(obj, Operation.Insert);
            return WriteDB(obj, Operation.Update);
        }
        /// <summary>
        /// Inserts or updates object - Archive mode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool InsertArchive(HotelDbElementBase obj)
        {
            return WriteDBArchive(obj, Operation.Insert);
        }
        /// <summary>
        /// Delete object from database
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Delete(HotelDbElementBase obj)
        {
            return WriteDB(obj, Operation.Delete);
        }

        /// <summary>
        /// JoinLists 2 lists based on ID correlation on the objects themselves
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="outerList"></param>
        /// <param name="innerList"></param>
        public static void JoinLists<T1, T2>(List<T1> outerList, List<T2> innerList) where T1 : HotelDbElementBase, new()
            where T2 : HotelDbElementBase, new()
        {
            string outerIdType = new T1().GetPrimaryKeyType();
            var inner_access = new T2();
            string innerIdType = inner_access.GetPrimaryKeyType();
            //Initial Id merge
            var mergedList = outerList.GroupJoin(innerList,
                outerkey => outerkey.GetPrimaryKey(),
                innerkey => innerkey.GetObjectProperty(innerkey, outerIdType),
                (outer, inner) => new
                {
                    Outer = outer,
                    Inner = inner
                });
            foreach (var outerObj in mergedList)
            {
                List<T2> matchList = new List<T2>();
                foreach (var innerObj in outerObj.Inner)
                {
                    innerObj.SetObjectProperty(innerObj, outerObj.Outer, outerObj.Outer.GetType().Name);
                    matchList.Add(innerObj);
                }
                outerObj.Outer.SetObjectList(outerObj.Outer, matchList, inner_access.GetType().Name + "List");
            }
        }
        /// <summary>
        /// Merge discrete objects into outer object from list
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="outerList"></param>
        /// <param name="innerList"></param>
        public static void JoinDiscreteByInner<T1, T2>(List<T1> outerList, List<T2> innerList) where T1 : HotelDbElementBase, new()
            where T2 : HotelDbElementBase, new()
        {
            T1 outer_access = new T1();
            T2 inner_access = new T2();
            PropertyInfo inner_property = inner_access.GetType().GetProperty(inner_access.GetPrimaryKeyType());
            PropertyInfo outer_property = outer_access.GetType().GetProperty(inner_access.GetPrimaryKeyType());
            foreach (var outerObj in outerList)
            {
                int innerid = (int)outer_property.GetValue(outerObj, null);
                foreach (var innerObj in innerList)
                {
                    if (innerid == innerObj.GetPrimaryKey())
                    {
                        outerObj.SetObjectProperty(outerObj, innerObj, innerObj.GetType().Name);
                        Debug.WriteLine("MATCH");
                    }
                }
            }
        }
        public static void JoinDiscreteByInnerOneWay<T1, T2>(List<T1> outerList, List<T2> innerList) where T1 : HotelDbElementBase, new()
            where T2 : HotelDbElementBase, new()
        {
            T1 outer_access = new T1();
            T2 inner_access = new T2();
            PropertyInfo inner_property = inner_access.GetType().GetProperty(inner_access.GetPrimaryKeyType());
            PropertyInfo outer_property = outer_access.GetType().GetProperty(inner_access.GetPrimaryKeyType());
            foreach (var outerObj in outerList)
            {
                int innerid = (int)outer_property.GetValue(outerObj, null);
                foreach (var innerObj in innerList)
                {
                    if (innerid == innerObj.GetPrimaryKey())
                    {
                        outerObj.SetObjectProperty(outerObj, innerObj, innerObj.GetType().Name);
                        Debug.WriteLine("MATCH");
                    }
                }
            }
        }
        public static void BackUpDb(string newPath)
        {
            File.Copy(dbFile, newPath, true);
        }
    }
}
