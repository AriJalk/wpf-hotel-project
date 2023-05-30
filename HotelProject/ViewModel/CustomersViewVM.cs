using HotelProject.Model.DbClasses;
using HotelProject.ViewModel.Commands;
using HotelProject.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HotelProject.ViewModel
{
    /// <summary>
    /// VM for CustomerView
    /// </summary>
    class CustomersViewVM : ViewModelBase, IPageViewModel
    {
        public string Name => "Customers";
        private bool _showbutton;

        public bool ShowButton
        {
            get { return _showbutton; }
            set
            {
                _showbutton = value;
                OnPropertyChanged("ShowButton");
            }
        }


        private bool[] _modeArray = new bool[] { true, false };
        /// <summary>
        /// Used for radio buttons to filter by phone/id
        /// </summary>
        public bool[] ModeArray
        {
            get { return _modeArray; }
        }
        public int SelectedMode
        {
            get { return Array.IndexOf(_modeArray, true); }
        }


        private ObservableCollection<Customer> _customerlist;
        /// <summary>
        /// List of all customers
        /// </summary>
        public ObservableCollection<Customer> CustomerList
        {
            get { return _customerlist; }
            set
            {
                _customerlist = value;
                OnPropertyChanged("CustomerList");
            }
        }

        private Customer _backupcustomer;

        private Customer _selectedcustomer;

        public Customer SelectedCustomer
        {
            get { return _selectedcustomer; }
            set
            {
                bool valid = true;
                //Save or revert object
                if (_selectedcustomer != null)
                {
                    if (_selectedcustomer.ValidateData() && IsUnique(_selectedcustomer))
                    {
                        SqlDatabaseHelper.Insert(_selectedcustomer);
                        _selectedcustomer.IsInDb = true;
                    }
                    else
                    {
                        MessageBox.Show("Info not correct, reverting");
                        if (_selectedcustomer.IsInDb)
                        {
                            _selectedcustomer = _backupcustomer;
                            valid = false;
                            Refresh();
                        }
                        else
                        {
                            _selectedcustomer.IdCount--;
                            _selectedcustomer = null;
                            valid = false;
                            Refresh();
                        }
                    }
                }
                if (value != null && valid)
                {
                    _selectedcustomer = value;
                    _backupcustomer = new Customer(value);
                }
                if (_selectedcustomer != null)
                    AppVm.SelectedCustomer = _selectedcustomer;
                OnPropertyChanged("SelectedCustomer");
                DeactivateCommand = new DeactivateCommand(this);
            }
        }


        private bool _showremoved = false;

        public bool ShowRemoved
        {
            get { return _showremoved; }
            set
            {
                _showremoved = value;
                OnPropertyChanged("ShowRemoved");
                Refresh();
            }
        }


        private ICollectionView _customercollection;
        /// <summary>
        /// Used to filter through customers
        /// </summary>
        public ICollectionView CustomerCollection
        {
            get
            {
                return _customercollection;
            }
            set
            {
                _customercollection = value;
                OnPropertyChanged("CustomerCollection");
            }
        }

        private string _searchstring = string.Empty;

        public string SearchString
        {
            get
            {
                return _searchstring;
            }
            set
            {
                _searchstring = value;
                OnPropertyChanged("PhoneString");
                CustomerCollection.Refresh();
            }
        }

        private RefreshCommand _refreshCommand;

        public RefreshCommand RefreshCommand
        {
            get { return _refreshCommand; }
            set
            {
                _refreshCommand = value;
                OnPropertyChanged("RefreshCommand");
            }
        }

        private DeactivateCommand _deactivatecommand;

        public DeactivateCommand DeactivateCommand
        {
            get { return _deactivatecommand; }
            set
            {
                _deactivatecommand = value;
                OnPropertyChanged("DeactivateCommand");
            }
        }



        public CustomersViewVM(ApplicationViewModel vm)
        {
            AppVm = vm;
            RefreshCommand = new RefreshCommand(this);
            DeactivateCommand = new DeactivateCommand(this);
        }

        private void InitializeVM()
        {
            IList<Customer> list = SqlDatabaseHelper.Read<Customer>();
            CustomerList = new ObservableCollection<Customer>(list);
            CustomerCollection = CollectionViewSource.GetDefaultView(list);
            if (SelectedMode == 0)
                CustomerCollection.Filter = FilterPhone;
            else if (SelectedMode == 1)
                CustomerCollection.Filter = FilterId;
            SelectedCustomer = null;
        }

        public void Refresh()
        {
            InitializeVM();
        }

        public void Refresh(Customer selctedCustomer)
        {
            InitializeVM();
            SelectedCustomer = selctedCustomer;
        }

        private bool FilterPhone(object obj)
        {
            Customer customer = obj as Customer;
            if (customer != null)
            {
                if (customer.IsActive == false && ShowRemoved == false)
                    return false;
                if (SearchString == string.Empty)
                    return true;
                if (customer.PhoneNumber != string.Empty)
                    if (customer.PhoneNumber.Contains(SearchString))
                        return true;
            }
            return false;
        }

        private bool FilterId(object obj)
        {
            Customer customer = obj as Customer;
            if (customer != null)
            {
                if (customer.IsActive == false && ShowRemoved == false)
                    return false;
                if (SearchString == string.Empty)
                    return true;
                if (customer.IdNumber != string.Empty)
                    if (customer.IdNumber.Contains(SearchString))
                        return true;
            }
            return false;
        }

        public void Dispose()
        {
            Debug.WriteLine("CustomerView Dispose");
        }

        public override void DeactivateSelectedItem()
        {
            if (SelectedCustomer != null)
                SelectedCustomer.IsActive = false;
            SqlDatabaseHelper.Insert(SelectedCustomer);
            Refresh();
        }

        bool IsUnique(Customer customer)
        {
            foreach (Customer other in CustomerList)
            {
                if (customer.Compare(other)) continue;
                if (customer.PhoneNumber == other.PhoneNumber || customer.IdNumber == other.IdNumber)
                    return false;
            }
            return true;
        }
    }
}
