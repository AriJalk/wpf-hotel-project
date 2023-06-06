using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace HotelProject.View.Helpers.Converters
{
    class PriceConverter : MarkupExtension, IValueConverter
    {
        private static PriceConverter _converter = null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            decimal price = (decimal)value;
            return price.ToString("C", CultureInfo.CurrentCulture);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal price = (decimal)value;
            if (price >=0&&price<=100000)
                return price;
            else
                return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new PriceConverter();
            return _converter;
        }
    }
}
