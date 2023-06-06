using HotelProject.Model.DbClasses;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace HotelProject.View.Helpers.Converters
{
    /// <summary>
    /// NOT version of IsManagerConverter
    /// </summary>
    class IsManagerNotConverter : MarkupExtension, IValueConverter
    {
        IsManagerNotConverter _converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            User user = value as User;
            if (user != null && user.UserType != null)
            {
                Debug.WriteLine("User Type is: " + user.UserType.Name);
                if (user.UserType.Name.Equals("Manager") || user.UserType.Name.Equals("Admin"))
                    return false;
            }
            Debug.WriteLine("CONVERTER TRUE");
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new IsManagerNotConverter();
            return _converter;
        }
    }
}
