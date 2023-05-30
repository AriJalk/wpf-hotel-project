using HotelProject.Model.BaseClasses;
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
    /// Converts person object to full name string
    /// </summary>
    class NumberedElementConverter : MarkupExtension, IValueConverter
    {
        NumberedElementConverter _converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HotelNumberedElementBase element = value as HotelNumberedElementBase;
            if (element!=null)
                return element.ElementNumber.ToString();
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new NumberedElementConverter();
            return _converter;
        }
    }
}
