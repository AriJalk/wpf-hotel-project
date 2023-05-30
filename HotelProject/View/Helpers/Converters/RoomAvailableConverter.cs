using HotelProject.Model.DbClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HotelProject.View.Helpers.Converters
{
    /// <summary>
    /// Returns color to represent availability, Green for available, Red for unavailable
    /// </summary>
    class RoomAvailableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isAvailable = (bool)value;
            if (isAvailable == true)
            {
                return new SolidColorBrush(Colors.Green);
            }

            return new SolidColorBrush(Colors.Red);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
