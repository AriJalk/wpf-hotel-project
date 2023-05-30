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
    class UserViewVM : ViewModelBase, IPageViewModel
    {
        public string Name => "Users";

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

        private ObservableCollection<User> _userlist;

        public ObservableCollection<User> UserList
        {
            get { return _userlist; }
            set
            {
                _userlist = value;
                OnPropertyChanged("UserList");
            }
        }

        private ObservableCollection<UserType> _usertypeslist;

        public ObservableCollection<UserType> UserTypesList
        {
            get { return _usertypeslist; }
            set
            {
                _usertypeslist = value;
                OnPropertyChanged("UserTypesList");
            }
        }

        private User _backupUser;

        private User _selectedUser;

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                bool valid = true;
                //Save or revert object
                if (_selectedUser != null)
                {
                    if (_selectedUser.ValidateData()&&IsUnique(_selectedUser))
                    {
                        //Test if tried to change self type
                        if (_selectedUser.UserId == AppVm.User.UserId)
                        {
                            valid = false;
                            _selectedUser.UserType = _backupUser.UserType;
                            MessageBox.Show("Can't chagne self type, change will not save");
                        }
                        else
                            SqlDatabaseHelper.Insert(_selectedUser);
                        _selectedUser.IsInDb = true;
                    }
                    else
                    {
                        MessageBox.Show("Info not correct, reverting");
                        if (_selectedUser.IsInDb)
                        {
                            if (_backupUser!=null)
                                _selectedUser = _backupUser;
                            valid = false;
                            Refresh();
                        }
                        else
                        {
                            _selectedUser.IdCount--;
                            _selectedUser = null;
                            valid = false;
                            Refresh();
                        }
                    }
                }
                if (value != null && valid)
                {
                    _selectedUser = value;
                    _backupUser = new User(value);
                }

                OnPropertyChanged("SelectedUser");
            }
        }

        private string _passwordstring;

        public string PasswordString
        {
            get { return _passwordstring; }
            set
            {
                _passwordstring = value;
                OnPropertyChanged("PasswordString");
            }
        }


        private ICollectionView _userscollection;

        public ICollectionView UsersCollection
        {
            get
            {
                return _userscollection;
            }
            set
            {
                _userscollection = value;
                OnPropertyChanged("UsersCollection");
            }
        }

        public User User
        {
            get { return AppVm.Globals.User; }
        }

        private ResetPasswordCommand _resetpasswordcommand;

        public ResetPasswordCommand ResetPasswordCommand
        {
            get { return _resetpasswordcommand; }
            set { _resetpasswordcommand = value; }
        }


        public UserViewVM(ApplicationViewModel vm)
        {
            AppVm = vm;
            ResetPasswordCommand = new ResetPasswordCommand(this);
        }


        private void InitializeVM()
        {
            List<User> users = SqlDatabaseHelper.Read<User>();
            SqlDatabaseHelper.JoinDiscreteByInner(users, AppVm.Globals.UserTypes);
            UserList = new ObservableCollection<User>(users);
            UserTypesList = new ObservableCollection<UserType>(AppVm.Globals.UserTypes);
            UsersCollection = CollectionViewSource.GetDefaultView(UserList);
            SelectedUser = null;
        }

        private void InitializeVM(User user)
        {
            List<User> users = SqlDatabaseHelper.Read<User>();
            SqlDatabaseHelper.JoinDiscreteByInner(users, AppVm.Globals.UserTypes);
            UserList = new ObservableCollection<User>(users);
            UserTypesList = new ObservableCollection<UserType>(AppVm.Globals.UserTypes);
            UsersCollection = CollectionViewSource.GetDefaultView(UserList);
            SelectedUser = user;
        }

        public void Dispose()
        {
            _selectedUser = null;
        }

        public void Refresh()
        {
            InitializeVM();
        }

        public void ResetPassword()
        {
            if (SelectedUser != null && !string.IsNullOrEmpty(PasswordString))
            {
                SelectedUser.HashedPassword = PasswordHelper.HashPassword(PasswordString);
                SqlDatabaseHelper.Insert(SelectedUser);
                InitializeVM(SelectedUser);
            }
        }
        bool IsUnique(User user)
        {
            foreach (User other in UserList)
            {
                if (user == other) continue;
                if (user.PhoneNumber == other.PhoneNumber || user.IdNumber == other.IdNumber)
                    return false;
            }
            return true;
        }
    }
}
