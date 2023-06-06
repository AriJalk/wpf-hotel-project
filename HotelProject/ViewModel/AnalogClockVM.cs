using HotelProject.Model.OtherClasses;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace HotelProject.ViewModel
{
    /// <summary>
    /// ClockVM
    /// https://github.com/Findlay999/AnalogClock-WPF
    /// </summary>
    class AnalogClockVM : ViewModelBase
    {
        public List<ClockPart> HourParts { get; set; }
        public List<ClockPart> SecondParts { get; set; }

        private DispatcherTimer Timer { get; set; } = new DispatcherTimer();

        private double angleHour;
        private double angleMin;
        private double angleSec;

        #region Get-Set
        public double AngleHour
        {
            get { return angleHour; }
            set
            {
                angleHour = value;
                OnPropertyChanged("AngleHour");
            }
        }


        public double AngleMin
        {
            get { return angleMin; }
            set
            {
                angleMin = value;
                OnPropertyChanged("AngleMin");
            }
        }

        public double AngleSec
        {
            get { return angleSec; }
            set
            {
                angleSec = value;
                OnPropertyChanged("AngleSec");
            }
        }
        #endregion

        #region Constructor

        public AnalogClockVM()
        {
            HourParts = new List<ClockPart>();
            for (int x = 0; x < 12; x++)
            {
                HourParts.Add(new ClockPart()
                {
                    Number = (x + 1).ToString(),
                    Angle = (x + 1) * (360 / 12),
                });
            }

            SecondParts = new List<ClockPart>();
            for (int x = 0; x < 60; x++)
            {
                SecondParts.Add(new ClockPart()
                {
                    Number = (x + 1).ToString(),
                    Angle = (x + 1) * (360 / 60),
                });
            }

            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }
        #endregion

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            AngleHour = (time.Hour) * (360 / 12);
            AngleMin = (time.Minute) * (360 / 60);
            AngleSec = (time.Second) * (360 / 60);
        }
    }
}
