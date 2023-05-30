using HotelProject.Model.BaseClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace HotelProject.View.Helpers.Converters
{
    /// <summary>
    /// Converts person object to first name
    /// </summary>
    class PersonToFNameConverter : MarkupExtension, IValueConverter
    {
        PersonToFNameConverter _converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Person person = value as Person;
            if (person != null)
            {
                return string.Format("{0}", person.FName);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new PersonToFNameConverter();
            return _converter;
        }
    }
}
