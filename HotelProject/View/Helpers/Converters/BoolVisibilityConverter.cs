using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace HotelProject.View.Helpers.Converters
{
    /// <summary>
    /// Converts bool to visibility property
    /// </summary>
    class BoolVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static BoolVisibilityConverter _converter = null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool obj = (bool)value;
            if (obj == true)
                return "Visible";
            return "Collapsed";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility vis = (Visibility)value;
            if (vis == Visibility.Visible)
                return true;
            else
                return false;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new BoolVisibilityConverter();
            return _converter;
        }
    }
}
