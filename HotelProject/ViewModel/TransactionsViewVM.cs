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
                    if (_selectedtransaction.ValidateData())
                    {
                        SqlDatabaseHelper.Insert(_selectedtransaction);
                        _selectedtransaction.IsInDb = true;
                    }
                    else
                    {
                        if (_selectedtransaction.IsInDb)
                        {
                            MessageBox.Show("Info not correct, reverting");
                            _selectedtransaction = _backuptransaction;
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


        public TransactionsViewVM(ApplicationViewModel parentvm)
        {
            AppVm = AppVm;
        }

        private void InitializeVM()
        {
            //Load, read and join all relational data regarding transactions
            List<RoomReservation> reservationlist = SqlDatabaseHelper.Read<RoomReservation>();
            List<Transaction> transactionlist = SqlDatabaseHelper.Read<Transaction>();
            List<TransactionPart> partListTemp = SqlDatabaseHelper.Read<TransactionPart>();
            List<Customer> customerListTemp=SqlDatabaseHelper.Read<Customer>();
            SqlDatabaseHelper.JoinDiscreteByInner(reservationlist, customerListTemp);
            SqlDatabaseHelper.JoinLists(transactionlist, partListTemp);
            SqlDatabaseHelper.JoinLists(reservationlist, transactionlist);
            SqlDatabaseHelper.JoinDiscreteByInner(transactionlist, SqlDatabaseHelper.Read<User>());
            List<Service> servicelist = SqlDatabaseHelper.Read<Service>();
            SqlDatabaseHelper.JoinDiscreteByInner(servicelist, SqlDatabaseHelper.Read<ServiceGroup>());
            SqlDatabaseHelper.JoinDiscreteByInnerOneWay(partListTemp, servicelist);
            SqlDatabaseHelper.JoinLists(transactionlist, partListTemp);
            TransactionList = new ObservableCollection<Transaction>(transactionlist);
            TransactionCollection = CollectionViewSource.GetDefaultView(transactionlist);
            TransactionCollection.Filter = null;
            SelectedTransaction = null;
            RefundCommand = new RefundCommand(this);
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
                Refresh();
            }
        }
    }
}
