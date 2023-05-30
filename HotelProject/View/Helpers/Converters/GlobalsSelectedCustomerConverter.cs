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
    class GlobalsSelectedCustomerConverter : MarkupExtension, IValueConverter
    {
        GlobalsSelectedCustomerConverter _converter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Customer cus= value as Customer;
            if (cus != null)
                return $"{cus.FName} {cus.LName}";
            else return "Customer Not Selected";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new GlobalsSelectedCustomerConverter();
            return _converter;
        }
    }
}
