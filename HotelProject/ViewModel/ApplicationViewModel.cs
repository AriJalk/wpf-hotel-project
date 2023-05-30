using HotelProject.Model.BaseClasses;
using HotelProject.Model.DbClasses;
using HotelProject.ViewModel.Commands;
using HotelProject.ViewModel.Helpers;
using HotelProject.ViewModel.Helpers.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
/// <summary>
/// Main menu navigation VM
/// source: https://rachel53461.wordpress.com/2011/12/18/navigation-with-mvvm-2/
/// </summary>

namespace HotelProject.ViewModel
{
    /// <summary>
    /// Main window VM
    /// </summary>
    public class ApplicationViewModel : ViewModelBase
    {
        #region Fields

        private ICommand _changePageCommand;

        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;

        #endregion

       

        #region Properties / Commands

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);
                }

                return _changePageCommand;
            }
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    if(_currentPageViewModel!=null)
                        _currentPageViewModel.Dispose();
                    _currentPageViewModel = value;
                    _currentPageViewModel.Refresh();
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }

        private User _user;

        public User User
        {
            get { return _user; }
            set 
            { 
                _user = value;
                OnPropertyChanged("User");
            }
        }

        private bool _isloggedin = false;

        public bool IsLoggedIn
        {
            get { return _isloggedin; }
            set 
            {
                _isloggedin = value;
                OnPropertyChanged("IsLoggedIn");
            }
        }


        private Uri _weatheruri= new Uri("https://www.israelweather.co.il/weather_out.asp?code_width=150&height=200&code_color=white&code_font_color=black");

        public Uri WeatherUri
        {
            get { return _weatheruri; }
            set { _weatheruri = value; }
        }

        private Globals _globals;

        public Globals Globals
        {
            get { return _globals; }
            set { _globals = value; }
        }



        #endregion
        public ApplicationViewModel()
        {
            //Add available pages (use with user controls)
            Globals = new Globals();
            Globals.UserTypes = SqlDatabaseHelper.Read<UserType>();
            Globals.RoomTypes = SqlDatabaseHelper.Read<RoomType>();
            PageViewModels.Add(new HomeViewVM(this));
            PageViewModels.Add(new FloorsViewVM(this));
            PageViewModels.Add(new CustomersViewVM(this));
            PageViewModels.Add(new TransactionsViewVM(this));
            PageViewModels.Add(new ServicesViewVM(this));
            PageViewModels.Add(new EditFloorRoomViewVM(this));
            PageViewModels.Add(new UserViewVM(this));
            PageViewModels.Add(new ReportsViewVM(this));
            PageViewModels.Add(new DbManagementViewVM());
            // Set starting page
            CurrentPageViewModel = PageViewModels[0];
        }

        #region Methods

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        public void LoginUser(User user)
        {
            if (user != null)
            {
                User = user;
                Globals.User = user;
                IsLoggedIn = true;
                //Show interface according to login
                foreach(IPageViewModel page in PageViewModels)
                {
                    if ((page.Name.Equals("Users")||page.Name.Equals("DB Management"))
                        &&!User.UserType.Name.Equals("Admin"))
                        page.ShowButton = false;
                    else if (page.Name.Equals("Floor/Room Edit") &&(!User.UserType.Name.Equals("Manager")&&!User.UserType.Name.Equals("Admin")))
                    {
                        page.ShowButton = false;
                    }
                    else
                        page.ShowButton = true;
                }
                RefreshVM();
            }
        }

        public void LogoutUser()
        {
            if (Globals.User != null)
            {
                Globals.User = null;
                User = null;
                IsLoggedIn = false;
                foreach(IPageViewModel page in PageViewModels)
                {
                    if (page.Name.Equals("Home Page")) 
                        page.ShowButton=true;
                    else page.ShowButton= false;
                }
            }
            RefreshVM();
        }

        private void RefreshVM()
        {
            
        }

        #endregion
    }
}
