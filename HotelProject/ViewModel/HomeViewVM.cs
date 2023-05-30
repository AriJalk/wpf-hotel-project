using HotelProject.Model.DbClasses;
using HotelProject.ViewModel.Commands;
using HotelProject.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelProject.ViewModel
{
    /// <summary>
    /// Home page VM with login
    /// </summary>
    public class HomeViewVM : ViewModelBase, IPageViewModel
    {
        public string Name => "Home Page";

        private bool _showbutton=true;

        public bool ShowButton
        {
            get { return _showbutton; }
            set
            {
                _showbutton = value;
                OnPropertyChanged("ShowButton");
            }
        }


        private LoginCommand _logincommand;
        public LoginCommand LoginCommand
        {
            get
            {
                return _logincommand;
            }
            set
            {
                _logincommand = value;
                OnPropertyChanged("LoginCommand");
            }
        }

        private LogoutCommand _logoutcommand;

        public LogoutCommand LogoutCommand
        {
            get { return _logoutcommand; }
            set
            { 
                _logoutcommand = value;
                OnPropertyChanged("LogoutCommand");
            }
        }


        private string _loginstring;

        public string LoginString
        {
            get { return _loginstring; }
            set { _loginstring = value; }
        }

        private string _passwordstring;

        public string PasswordString
        {
            get { return _passwordstring; }
            set { _passwordstring = value; }
        }

        private bool _isloggedin;

        public bool IsLoggedIn
        {
            get { return _isloggedin; }
            set 
            {
                _isloggedin = value;
                OnPropertyChanged("IsLoggedIn");
            }
        }




        public HomeViewVM(ApplicationViewModel vm)
        {
            LoginCommand = new LoginCommand(this);
            LogoutCommand = new LogoutCommand(this);
            AppVm = vm;
        }

        public string LogoPath
        {
            get { return ObjectFileHelper.GetFullPath(@"\Resources\Images\HotelLogo.png"); }
        }


        public void Login()
        {
            //Test if fields of login and password are empty
            if (string.IsNullOrEmpty(LoginString) || string.IsNullOrEmpty(PasswordString))
                MessageBox.Show("Login information missing");
            else
            {
                //Read user from db
                User user = SqlDatabaseHelper.ReadSingle<User>("Login='" + LoginString + "' " 
                    + "AND IsActive=YES");
                if (user.IsInDb)
                {
                    //Read user types and match
                    List<UserType> types = SqlDatabaseHelper.Read<UserType>();
                    foreach (var type in types)
                    {
                        if (user.UserTypeId == type.UserTypeId)
                            user.UserType = type;
                    }
                    //Login user if details are correct
                    if (PasswordHelper.ValidatePassword(PasswordString, user.HashedPassword))
                    {
                        Debug.WriteLine(user.FName + " " + user.LName + " Logged In");
                        AppVm.LoginUser(user);
                        IsLoggedIn = AppVm.IsLoggedIn;
                        Refresh();
                    }
                    else
                        MessageBox.Show("Login Details Invalid");
                }
                else
                    MessageBox.Show("LoginDetailsInvalid");
            }
        }

        public void Logout()
        {
            AppVm.LogoutUser();
            IsLoggedIn = AppVm.IsLoggedIn;
            Refresh();
        }

        public void Dispose()
        {
            Debug.WriteLine("HomeView Dispose");
            LoginString = string.Empty;
            PasswordString = string.Empty;
        }

        //Refresh text boxes
        public void Refresh()
        {
            LoginString = string.Empty;
            PasswordString = string.Empty;
        }
    }
}
