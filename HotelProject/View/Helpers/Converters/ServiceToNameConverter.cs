using HotelProject.Model.DbClasses;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace HotelProject.View.Helpers.Converters
{
    /// <summary>
    /// Converts service object to its name
    /// </summary>
    class ServiceToNameConverter : MarkupExtension, IValueConverter
    {
        private static ServiceToNameConverter _converter = null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Service obj = (Service)value;
            if (obj != null)
                return obj.Name;
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new ServiceToNameConverter();
            return _converter;
        }
    }
}
