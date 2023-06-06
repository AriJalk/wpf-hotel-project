using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

//Template taken online for ViewModelBase
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
            //Debug.WriteLine(propertyName + " CHANGED");
        }

        public virtual void OnPropertyChangedUserControl(object obj,string propertyName)
        {
            PropertyChanged?.Invoke(obj, new PropertyChangedEventArgs(propertyName));
            //Debug.WriteLine(propertyName + " CHANGED");
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
