using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject.Model.OtherClasses
{
    /// <summary>
    /// Dummy class for emulating a payment service that validates credit cards
    /// </summary>
    public static class PaymentServiceDummy
    {
        public static bool ValidateCard(string creditCard, int month, int year, int ccv)
        {
            if (creditCard != string.Empty && month > 0 && month <= 12 && year >= DateTime.Now.Year && ccv > 0 && ccv < 1000)
                return true;
            return false;
        }
    }
}
