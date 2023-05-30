using HotelProject.Model.DbClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace HotelProject.View.Helpers.Converters
{
    /// <summary>
    /// Relative size according to container
    /// </summary>
    class WindowSizeConverter : MarkupExtension,IValueConverter
    {
        private static WindowSizeConverter _converter = null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * ((double)parameter / 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new WindowSizeConverter();
            return _converter;
        }
    }
}
