using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace slobus_v1._0_db.ViewModels
{
    public class RouteStopViewModel : INotifyPropertyChanged
    {
        private DateTime _time;
        /// <summary>
        /// ViewModel property; this property is used to identify the route.
        /// </summary>
        /// <returns></returns>
        public DateTime Time
        {
            get
            {
                return _time;
            }
            set
            {
                if (value != _time)
                {
                    _time = value;
                    NotifyPropertyChanged("Time");
                }
            }
        }

        private TimeSpan _timeUntil;
        /// <summary>
        /// ViewModel property; this property is used to identify the route.
        /// </summary>
        /// <returns></returns>
        public TimeSpan TimeUntil
        {
            get
            {
                return _timeUntil;
            }
            set
            {
                if (value != _timeUntil)
                {
                    _timeUntil = value;
                    NotifyPropertyChanged("TimeUntil");
                }
            }
        }

        private string _routeId;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string RouteID
        {
            get
            {
                return _routeId;
            }
            set
            {
                if (value != _routeId)
                {
                    _routeId = value;
                    NotifyPropertyChanged("RouteID");
                }
            }
        }

        private string _stopId;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string StopID
        {
            get
            {
                return _stopId;
            }
            set
            {
                if (value != _stopId)
                {
                    _stopId = value;
                    NotifyPropertyChanged("StopID");
                }
            }
        }

        private Boolean _weekday;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public Boolean Weekday
        {
            get
            {
                return _weekday;
            }
            set
            {
                if (value != _weekday)
                {
                    _weekday = value;
                    NotifyPropertyChanged("Weekday");
                }
            }
        }

        private Boolean _saturday;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public Boolean Saturday
        {
            get
            {
                return _saturday;
            }
            set
            {
                if (value != _saturday)
                {
                    _saturday = value;
                    NotifyPropertyChanged("Saturday");
                }
            }
        }

        private Boolean _sunday;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public Boolean Sunday
        {
            get
            {
                return _sunday;
            }
            set
            {
                if (value != _sunday)
                {
                    _sunday = value;
                    NotifyPropertyChanged("Sunday");
                }
            }
        }

        private DateTime _dateStart;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public DateTime DateStart
        {
            get
            {
                return _dateStart;
            }
            set
            {
                if (value != _dateStart)
                {
                    _dateStart = value;
                    NotifyPropertyChanged("DateStart");
                }
            }
        }

        private DateTime _dateEnd;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public DateTime DateEnd
        {
            get
            {
                return _dateEnd;
            }
            set
            {
                if (value != _dateEnd)
                {
                    _dateEnd = value;
                    NotifyPropertyChanged("DateEnd");
                }
            }
        }

        private Boolean _busB;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public Boolean BusB
        {
            get
            {
                return _busB;
            }
            set
            {
                if (value != _busB)
                {
                    _busB = value;
                    NotifyPropertyChanged("busB");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
