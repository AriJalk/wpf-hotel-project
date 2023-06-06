using HotelProject.Model.DbClasses;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace HotelProject.View.Helpers.Converters
{
    /// <summary>
    /// Visibility converter for manager and admin
    /// </summary>
    class UserVisibilityConverter : MarkupExtension,IValueConverter
    {
        private static UserVisibilityConverter _converter = null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            User user = value as User;
            if (user != null)
            {
                if (user.UserType.Name == "Manager"||user.UserType.Name=="Admin")
                    return "Visible";
            }
            return "Collapsed";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new UserVisibilityConverter();
            return _converter;
        }
    }
}
