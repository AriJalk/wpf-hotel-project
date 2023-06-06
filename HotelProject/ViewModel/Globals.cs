using HotelProject.Model.DbClasses;
using System.Collections.Generic;

namespace HotelProject.ViewModel
{   //Singleton object to contain global parameters like current user or customer
    public class Globals 
    {

        private User _user;

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }

        private Customer _selectedcustomer;

        public Customer SelectedCustomer
        {
            get
            {
                return _selectedcustomer;
            }
            set
            {
                if(value!=null)
                    _selectedcustomer = value;
            }
        }

        private Room _selectedroom;

        public Room SelectedRoom
        {
            get { return _selectedroom; }
            set 
            { 
                if(value!=null)
                    _selectedroom = value;
            }
        }
        private Floor _selectedfloor;

        public Floor SelectedFloor
        {
            get { return _selectedfloor; }
            set
            {
                if (value != null)
                    _selectedfloor = value;
            }
        }


        private List<UserType> _usertypes;
        public List<UserType> UserTypes
        {
            get
            {
                return _usertypes;
            }
            set
            {
                _usertypes = value;
            }
        }

        private List<RoomType> _roomtypes;

        public List<RoomType> RoomTypes
        {
            get { return _roomtypes; }
            set { _roomtypes = value; }
        }


        public Globals() { }
       
    }
}
