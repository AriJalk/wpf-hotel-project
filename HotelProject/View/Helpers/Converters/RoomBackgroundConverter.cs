using HotelProject.Model.DbClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace HotelProject.View.Helpers.Converters
{
    /// <summary>
    /// Converts room object to background color based on room type
    /// </summary>
    class RoomBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int type = (int)value;
            if (type == 2)
                return "#DEC143";
            else if (type == 1)
                return "#72E872";
            else if (type == 3)
                return new SolidColorBrush(Colors.LightPink);
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
