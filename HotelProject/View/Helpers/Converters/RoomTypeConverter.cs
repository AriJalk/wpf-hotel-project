using HotelProject.Model.DbClasses;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace HotelProject.View.Helpers.Converters
{
    /// <summary>
    /// RoomType<->Name converter
    /// </summary>
    class RoomTypeConverter : MarkupExtension, IValueConverter
    {
        private static RoomTypeConverter _converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RoomType type = value as RoomType;
            if (type != null)
                return type.Name;
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RoomType type = value as RoomType;
            if (type != null)
                return type;
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new RoomTypeConverter();
            return _converter;
        }
    }
}
