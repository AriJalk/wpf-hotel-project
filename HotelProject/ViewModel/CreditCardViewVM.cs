using HotelProject.Model.DbClasses;
using HotelProject.View;
using HotelProject.ViewModel.Commands.CreditCardCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelProject.ViewModel
{
    /// <summary>
    /// VM for CreditCard payment window
    /// </summary>
    public class CreditCardViewVM : ViewModelBase
    {
        private decimal _total;
        /// <summary>
        /// Total for order
        /// </summary>
        public decimal Total
        {
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged("Total");
            }
        }


        private Transaction _transaction;

        public CreditCardViewVM()
        {

        }

        private string _creditcard;
        /// <summary>
        /// Store creditcard number
        /// </summary>
        public string CreditCardNumber
        {
            get { return _creditcard; }
            set 
            { 
                _creditcard = value;
                OnPropertyChanged("CreditCardNumber");
            }
        }

        private int _month;
        /// <summary>
        /// Expiration month
        /// </summary>
        public string Month
        {
            get { return _month.ToString(); }
            set
            {
                _month = int.Parse(value);
                OnPropertyChanged("Month");
            }
        }

        private int _year;
        /// <summary>
        /// Expiratoin year
        /// </summary>
        public string Year
        {
            get { return _year.ToString(); }
            set 
            { 
                _year = int.Parse(value);
                OnPropertyChanged("Year");
            }
        }

        private int _ccv;

        public string CCV
        {
            get { return _ccv.ToString(); }
            set 
            {
                _ccv = int.Parse(value);
                OnPropertyChanged("CCV");
            }
        }

        private CreditCardView _view;

        public PayCommand PayCommand { get; set; }



        public CreditCardViewVM(Transaction transaction,CreditCardView view)
        {
            _transaction = transaction;
            Total = transaction.ToPayAmount;
            PayCommand = new PayCommand(this);
            _view = view;
        }



        public void Pay()
        {
            _transaction.IsValidated = true;
            _transaction.IsPayed = true;
            _view.Close();
            if (ValidateCard())
            {
                _transaction.IsPayed = true;
                _view.Close();
            }
            else
                MessageBox.Show("Credit Not Valid");
        }
        
        private bool ValidateCard()
        {
            if (PaymentServiceDummy(CreditCardNumber, _month, _year, _ccv))
            {
                _transaction.IsValidated = true;
                return true;
            }

            return false;
        }
        //Dummy validation service
        private bool PaymentServiceDummy(string creditCard, int month, int year, int ccv)
        {
            if (creditCard != string.Empty && month > 0 && month <= 12 && year >= DateTime.Now.Year && ccv > 0 && ccv < 1000)
                return true;
            return false;
        }
    }
}
