using HotelProject.Model.DbClasses;
using HotelProject.ViewModel.Commands.TransactionCommands;
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
    class TransactionsViewVM : ViewModelBase, IPageViewModel
    {

        public string Name => "Transactions";

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


        private ObservableCollection<Transaction> _transactionlist;

        public ObservableCollection<Transaction> TransactionList
        {
            get { return _transactionlist; }
            set
            {
                _transactionlist = value;
                OnPropertyChanged("TransactionList");
            }
        }

        private ObservableCollection<TransactionPart> _partlist;

        public ObservableCollection<TransactionPart> PartList
        {
            get { return _partlist; }
            set
            {
                _partlist = value;
                OnPropertyChanged("PartList");
            }
        }


        private Transaction _backuptransaction;

        private Transaction _selectedtransaction;

        public Transaction SelectedTransaction
        {
            get { return _selectedtransaction; }
            set
            {
                bool valid = true;
                //Save or revert object
                if (_selectedtransaction != null)
                {
                    if (_selectedtransaction.ValidateData() && _selectedtransaction.RoomReservation.ValidateData())
                    {

                        SqlDatabaseHelper.Insert(_selectedtransaction);
                        SqlDatabaseHelper.Insert(_selectedtransaction.RoomReservation);
                        _selectedtransaction.IsInDb = true;

                    }
                    else
                    {
                        if (_selectedtransaction.IsInDb)
                        {
                            MessageBox.Show("Info not correct, reverting");
                            _selectedtransaction = new Transaction(_backuptransaction);
                            valid = false;
                            Refresh();
                        }
                        else
                        {
                            _selectedtransaction.IdCount--;
                            _selectedtransaction = null;
                            valid = false;
                            Refresh();
                        }
                    }
                }
                if (value != null && valid)
                {
                    _selectedtransaction = value;
                    _backuptransaction = new Transaction(value);
                    PartList = new ObservableCollection<TransactionPart>(_selectedtransaction.TransactionPartList);
                }
                if (value == null)
                {
                    _selectedtransaction = null;
                }
                OnPropertyChanged("SelectedTransaction");
            }
        }

        private TransactionPart _selectedpart;

        public TransactionPart SelectedPart
        {
            get { return _selectedpart; }
            set
            {
                _selectedpart = value;
                OnPropertyChanged("SelectedPart");
            }
        }

        private bool _isArchive;

        public bool IsArchive
        {
            get { return _isArchive; }
            set
            {
                _isArchive = value;
                Refresh();
                OnPropertyChanged("IsArchive");
            }
        }


        private ICollectionView _transactioncollection;
        public ICollectionView TransactionCollection
        {
            get
            {
                return _transactioncollection;
            }
            set
            {
                _transactioncollection = value;
                OnPropertyChanged("TransactionCollection");
            }
        }

        private ICollectionView _partcollection;

        public ICollectionView PartCollection
        {
            get { return _partcollection; }
            set
            {
                _partcollection = value;
                OnPropertyChanged("PartCollection");
            }
        }

        private RefundCommand _refundcommand;

        public RefundCommand RefundCommand
        {
            get { return _refundcommand; }
            set
            {
                _refundcommand = value;
                OnPropertyChanged("RefundCommand");
            }
        }

        private ArchiveTransactionsCommand _archiveTransactionsCommand;

        public ArchiveTransactionsCommand ArchiveTransactionsCommand
        {
            get { return _archiveTransactionsCommand; }
            set
            {
                _archiveTransactionsCommand = value;
                OnPropertyChanged("ArchiveTransactionsCommand");
            }
        }

        private bool _isfilter;

        public bool IsFilter
        {
            get { return _isfilter; }
            set
            {
                _isfilter = value;
                OnPropertyChanged("IsFilter");
                TransactionCollection.Refresh();
                //Refresh();
            }
        }

        private bool _isRegular;

        public bool IsRegular
        {
            get { return _isRegular; }
            set
            {
                _isRegular = value;
                Refresh();
                OnPropertyChanged("IsRegular");
            }
        }

        private bool _isToday;

        public bool IsToday
        {
            get { return _isToday; }
            set
            {
                _isToday = value;
                //Refresh();
                OnPropertyChanged("IsToday");
                TransactionCollection.Refresh();
            }
        }


        private bool _isCustomerSelected;

        public bool IsCustomerSelected
        {
            get { return _isCustomerSelected; }
            set
            {
                _isCustomerSelected = value;
                OnPropertyChanged("IsUserSelected");
            }
        }


        public TransactionsViewVM(ApplicationViewModel parentvm)
        {
            AppVm = parentvm;
        }

        private void InitializeVM()
        {
            List<Transaction> transactionlist;
            List<TransactionPart> partListTemp;
            //Load, read and join all relational data regarding transactions
            List<RoomReservation> reservationList = SqlDatabaseHelper.Read<RoomReservation>();

            if (IsArchive)
            {
                if (IsRegular)
                {
                    transactionlist = SqlDatabaseHelper.Read<Transaction>();
                    transactionlist.AddRange(SqlDatabaseHelper.ReadArchive<Transaction>());
                    partListTemp = SqlDatabaseHelper.Read<TransactionPart>();
                    partListTemp.AddRange(SqlDatabaseHelper.ReadArchive<TransactionPart>());
                }
                else
                {
                    transactionlist = SqlDatabaseHelper.ReadArchive<Transaction>();
                    partListTemp = SqlDatabaseHelper.ReadArchive<TransactionPart>();
                }
            }
            else
            {
                transactionlist = SqlDatabaseHelper.Read<Transaction>();
                partListTemp = SqlDatabaseHelper.Read<TransactionPart>();
            }

            List<Customer> customerListTemp = SqlDatabaseHelper.Read<Customer>();
            SqlDatabaseHelper.JoinDiscreteByInner(reservationList, customerListTemp);
            SqlDatabaseHelper.JoinLists(transactionlist, partListTemp);
            SqlDatabaseHelper.JoinLists(reservationList, transactionlist);
            SqlDatabaseHelper.JoinDiscreteByInner(transactionlist, SqlDatabaseHelper.Read<User>());
            List<Service> servicelist = SqlDatabaseHelper.Read<Service>();
            SqlDatabaseHelper.JoinDiscreteByInner(servicelist, SqlDatabaseHelper.Read<ServiceGroup>());
            SqlDatabaseHelper.JoinDiscreteByInnerOneWay(partListTemp, servicelist);
            SqlDatabaseHelper.JoinLists(transactionlist, partListTemp);
            SqlDatabaseHelper.JoinDiscreteByInnerOneWay(reservationList, SqlDatabaseHelper.Read<Room>());
            TransactionList = new ObservableCollection<Transaction>(transactionlist);
            TransactionCollection = CollectionViewSource.GetDefaultView(transactionlist);
            if (AppVm.Globals.SelectedCustomer != null)
                IsCustomerSelected = true;
            else 
                IsCustomerSelected = false;
            TransactionCollection.Filter = Filter;
            SelectedTransaction = null;
            RefundCommand = new RefundCommand(this);
            ArchiveTransactionsCommand = new ArchiveTransactionsCommand(this);
        }

        public void Refresh()
        {
            InitializeVM();
        }

        public void Refresh(Transaction selctedTransaction)
        {
            InitializeVM();
            SelectedTransaction = selctedTransaction;
        }

        public void Dispose()
        {
            _selectedtransaction = null;
            Debug.WriteLine("TransactionView Dispose");
        }

        public void RefundSelectedTransaction()
        {
            if (SelectedTransaction != null)
            {
                SelectedTransaction.Refund();
                SqlDatabaseHelper.Insert(SelectedTransaction);
                SqlDatabaseHelper.Insert(SelectedTransaction.RoomReservation);
                foreach (TransactionPart part in SelectedTransaction.TransactionPartList)
                    SqlDatabaseHelper.Insert(part);
                TransactionCollection.Refresh();
            }
        }

        public void Archive()
        {
            foreach (Transaction transaction in TransactionList)
                transaction.Archive();
            Refresh();
        }
        
        private bool Filter(object obj)
        {
            Transaction transaction = obj as Transaction;
            if (!IsToday && !IsFilter)
                return true;
            if (IsToday)
            {
                if (IsFilter)
                {
                    if ((transaction.RoomReservation.StartTime.Date == DateTime.Today|| transaction.RoomReservation.StartTime.Date == DateTime.Today.AddDays(-1)) &&
                        transaction.Customer.IdNumber == AppVm.Globals.SelectedCustomer.IdNumber)
                        return true;
                }
                else
                {
                    if (transaction.RoomReservation.StartTime.Date == DateTime.Today|| transaction.RoomReservation.StartTime.Date == DateTime.Today.AddDays(-1))
                        return true;
                }
            }
            else
            {
                if (IsFilter)
                {
                    if (transaction.Customer.IdNumber==AppVm.Globals.SelectedCustomer.IdNumber)
                        return true;
                }
            }
            return false;
        }
    }
}
