using HotelProject.ViewModel.Commands.DbManagement;
using HotelProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using HotelProject.ViewModel.Helpers;

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

        public ICommand BackupCommand { get; set; }

        public string Name => "DB Management";

        public DbManagementViewVM()
        {
            CreateTable = new CreateTablesCommand();
            DeleteAllTables = new DeleteAllTablesCommand();
            BackupCommand = new BackupDbCommand(this);

        }

        public void Refresh()
        {
            //TODO
        }

        public void Dispose()
        {
            Debug.WriteLine("DbManagement Dispose");
        }

        public void BackupDB()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.CurrentDirectory + @"\Backup";
            saveFileDialog.FileName = $"DB_{DateTime.Now.Year}{DateTime.Now.Month.ToString().PadLeft(2, '0')}{DateTime.Now.Day.ToString().PadLeft(2, '0')}";
            saveFileDialog.DefaultExt = ".accdb";
            saveFileDialog.Filter = "Access Database (.accdb)|*.accdb";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                SqlDatabaseHelper.BackUpDb(saveFileDialog.FileName);
        }
    }
}
