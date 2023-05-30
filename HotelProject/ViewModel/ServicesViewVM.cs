using HotelProject.Model.DbClasses;
using HotelProject.ViewModel.Commands;
using HotelProject.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelProject.ViewModel
{
    /// <summary>
    /// VM for ServiceView
    /// </summary>
    public class ServicesViewVM : ViewModelBase, IPageViewModel
    {

        public string Name => "ServiceList";

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

        private ObservableCollection<ServiceGroup> _servicegroupcollection;

        public ObservableCollection<ServiceGroup> ServiceGroupCollection
        {
            get 
            {
                return _servicegroupcollection; 
            }
            set
            {
                _servicegroupcollection = value;
                OnPropertyChanged("ServiceGroupCollection");
            }
        }

        private bool _iseditactive;

        public bool IsEditActive
        {
            get 
            { 
                return _iseditactive; 
            }
            set 
            { 
                _iseditactive = value;
                OnPropertyChanged("IsEditActive");
            }
        }

        private ServiceGroup _selectedservicegroup;
        public ServiceGroup SelectedServiceGroup
        {
            get
            {
                return _selectedservicegroup;
            }
            set
            {
                if (value != null)
                {
                    _selectedservicegroup = value;
                    ServicesInGroup.Clear();
                    foreach (Service service in _selectedservicegroup.ServiceList)
                    {
                        ServicesInGroup.Add(service);
                    }
                }
                OnPropertyChanged("SelectedServiceGroup");
            }
        }

        private Service _selectedservice;

        public Service SelectedService
        {
            get { return _selectedservice; }
            set 
            {
                if (value != null)
                {
                    _selectedservice = value;
                }
                OnPropertyChanged("SelectedService");
            }
        }


        private ObservableCollection<Service> _servicesingroup;

        public ObservableCollection<Service> ServicesInGroup
        {
            get { return _servicesingroup; }
            set 
            { 
                _servicesingroup = value;
                OnPropertyChanged("ServicesInGroup");
            }
        }


        public ServicesViewVM(ApplicationViewModel vm)
        {
            AppVm = vm;
            
        }

        public void InitializeVM()
        {
            List<ServiceGroup> groups = SqlDatabaseHelper.Read<ServiceGroup>();
            List<Service> services = SqlDatabaseHelper.Read<Service>();
            SqlDatabaseHelper.JoinLists(groups, services);
            ServiceGroupCollection = new ObservableCollection<ServiceGroup>(groups);
            IsEditActive = false;
            ServicesInGroup = new ObservableCollection<Service>();
            SelectedServiceGroup = null;
        }

        public void Refresh()
        {
            InitializeVM();
        }


        public void Dispose()
        {
            Debug.WriteLine("ServiceView Dispose");
            foreach(ServiceGroup group in ServiceGroupCollection)
            {
                //Save changes to prices
                foreach(Service service in group.ServiceList)
                {
                    //Save price
                    if (service.Price > 0)
                        SqlDatabaseHelper.Insert(service);
                }
            }
        }
    }
}
