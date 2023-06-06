using HotelProject.Model.DbClasses;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace HotelProject.View.Helpers.Converters
{
    /// <summary>
    /// Convertrs UserType<->Name
    /// </summary>
    class UserTypeConverter : MarkupExtension, IValueConverter
    {
        private static UserTypeConverter _converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UserType type = value as UserType;
            if (type != null)
                return type.Name;
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UserType type = value as UserType;
            if (type != null)
                return type;
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new UserTypeConverter();
            return _converter;
        }
    }
}
