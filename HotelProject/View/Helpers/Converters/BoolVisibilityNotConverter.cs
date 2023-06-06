using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace HotelProject.View.Helpers.Converters
{
    /// <summary>
    /// NOT version of BoolVisibilityConverter
    /// </summary>
    class BoolVisibilityNotConverter : MarkupExtension, IValueConverter
    {
        private static BoolVisibilityNotConverter _converter = null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool obj = (bool)value;
            if (obj == true)
                return Visibility.Collapsed;
            return Visibility.Visible;
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
            if (_converter == null) _converter = new BoolVisibilityNotConverter();
            return _converter;
        }
    }
}
