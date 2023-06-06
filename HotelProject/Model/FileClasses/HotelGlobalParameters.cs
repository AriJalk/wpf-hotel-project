namespace HotelProject.Model.FileClasses
{
    /// <summary>
    /// Global hotel parameters class
    /// </summary>
    public class HotelGlobalParameters
    {
        private int _checkinhour;
        /// <summary>
        /// Default check-in hour
        /// </summary>
        public int CheckInHour
        {
            get
            { 
                return _checkinhour;
            }
            set
            {
                if (value >= 0 && value <= 23)
                    _checkinhour = value;
                else _checkinhour = -1;
            }
        }

        private int _checkinminutes;
        /// <summary>
        /// Default check-in minute
        /// </summary>
        public int CheckInMinutes
        {
            get
            {
                return _checkinminutes;
            }
            set
            {
                if (value >= 0 && value <= 59)
                    _checkinminutes = value;
                else _checkinminutes = -1;
            }
        }


        private int _checkouthour;
        /// <summary>
        /// Default check-out hour
        /// </summary>
        public int CheckOutHour
        {
            get { return _checkouthour; }
            set
            {
                if (value >= 0 && value <= 23)
                    _checkouthour = value;
                else _checkinhour = -1;
            }
        }
        private int _checkoutminutes;
        /// <summary>
        /// Default check-out minute
        /// </summary>
        public int CheckOutMinutes
        {
            get
            {
                return _checkoutminutes;
            }
            set
            {
                if (value >= 0 && value <= 59)
                    _checkoutminutes = value;
                else _checkoutminutes = -1;
            }
        }


        public HotelGlobalParameters()
        {
            
        }

    }
}
