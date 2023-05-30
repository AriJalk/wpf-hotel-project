using HotelProject.Model.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject.ViewModel.Helpers
{
    static class Converters
    {
        public static string BoolToTable(bool source)
        {
            if (source == true)
                return "-1";
            else return "0";
        }
    }
}
