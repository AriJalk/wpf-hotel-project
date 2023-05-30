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
    class PersonNameConverter : MarkupExtension, IValueConverter
    {
        PersonNameConverter _converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Person person = value as Person;
            if (person != null)
                return string.Format("{0} {1}", person.FName, person.LName);
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new PersonNameConverter();
            return _converter;
        }
    }
}
