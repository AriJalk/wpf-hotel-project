using HotelProject.Model.BaseClasses;
using HotelProject.Model.DbClasses;
using HotelProject.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelProject.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ApplicationViewModel _appvm;

        public ApplicationViewModel AppVm
        {
            get { return _appvm; }
            set { _appvm = value; }
        }


        public ViewModelBase()
        {

        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Debug.WriteLine(propertyName + " CHANGED");
        }

        public virtual void OnPropertyChangedUserControl(object obj,string propertyName)
        {
            PropertyChanged?.Invoke(obj, new PropertyChangedEventArgs(propertyName));
            Debug.WriteLine(propertyName + " CHANGED");
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        public virtual void IncrementProperty()
        {

        }

        public virtual void DecrementProperty()
        {

        }
        public virtual void DeactivateSelectedItem()
        {

        }
    }
}
