using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace HotelProject.ViewModel.Helpers
{
    static class ObjectFileHelper
    {
        public static bool WriteObjectToFile(object obj)
        {
            string currentdir = Directory.GetCurrentDirectory();
            string filepath = currentdir+ @"\PropertyFiles\"+ obj.GetType().Name+".json";
            if(!Directory.Exists("PropertyFiles"))
                Directory.CreateDirectory("PropertyFiles");
            try
            {
                using (StreamWriter file = File.CreateText(filepath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serialize object directly into file stream
                    serializer.Serialize(file, obj);
                    return true;
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("Exception: " + err.Message);
            }
            return false;
        }

        public static object ReadObjectFromFile<T>(string folder, string filename) where T : new()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filePath = currentDir + @"\" + folder + @"\" + filename;
            try
            {
                string jsonString = File.ReadAllText(filePath + ".json");
                T newobj = new T();
                JsonSerializer serializer = new JsonSerializer();
                newobj = JsonConvert.DeserializeObject<T>(jsonString);
                if (newobj != null)
                    return newobj;
            }
            catch (Exception err)
            {
                Debug.WriteLine("Exception: " + err.Message);
            }
            return null;
        }

        public static string GetFullPath(string relativePath)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string filePath = currentDir + relativePath;
            return filePath;
        }
    }
}
