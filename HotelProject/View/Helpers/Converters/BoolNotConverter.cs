using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace HotelProject.View.Helpers.Converters
{
    /// <summary>
    /// Converts a bool to its NOT state
    /// </summary>
    class BoolNotConverter : MarkupExtension, IValueConverter
    {

        private static BoolNotConverter _converter = null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool obj = (bool)value;
            if (obj == true)
                return false;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new BoolNotConverter();
            return _converter;
        }
    }
}
