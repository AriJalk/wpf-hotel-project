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
    /// Checks for usertype and return true
    /// for manager and admin
    /// </summary>
    class IsManagerConverter : MarkupExtension, IValueConverter
    {
        IsManagerConverter _converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            User user = value as User;
            if (user != null&&user.UserType!=null)
            {
                Debug.WriteLine("User Type is: " + user.UserType.Name);
                if (user.UserType.Name == "Manager"||user.UserType.Name=="Admin")
                    return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new IsManagerConverter();
            return _converter;
        }
    }
}
