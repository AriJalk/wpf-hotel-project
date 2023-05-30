using HotelProject.ViewModel.Commands.DbManagement;
using HotelProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;

namespace HotelProject.ViewModel
{
    /// <summary>
    /// Development only
    /// Deletes and creates tables
    /// </summary>
    public class DbManagementViewVM : ViewModelBase, IPageViewModel
    {
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
        public ICommand CreateTable { get; set; }

        public ICommand DeleteAllTables { get; set; }

        public string Name => "DB Management";

        public DbManagementViewVM()
        {
            CreateTable = new CreateTablesCommand();
            DeleteAllTables = new DeleteAllTablesCommand();

        }

        public void Refresh()
        {
            //TODO
        }

        public void Dispose()
        {
            Debug.WriteLine("DbManagement Dispose");
        }
    }
}
