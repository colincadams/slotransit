using Microsoft.Phone.Maps.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace slobus_v1._0_db.ViewModels
{
    public class RouteViewModel : INotifyPropertyChanged
    {
        private string _routeId;
        /// <summary>
        /// ViewModel property; this property is used to identify the route.
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

        private string _name;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _shortName;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string ShortName
        {
            get
            {
                return _shortName;
            }
            set
            {
                if (value != _shortName)
                {
                    _shortName = value;
                    NotifyPropertyChanged("ShortName");
                }
            }
        }

        private string _desc;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Desc
        {
            get
            {
                return _desc;
            }
            set
            {
                if (value != _desc)
                {
                    _desc = value;
                    NotifyPropertyChanged("Desc");
                }
            }
        }

        private string _notRunningMessage;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string NotRunningMessage
        {
            get
            {
                return _notRunningMessage;
            }
            set
            {
                if (value != _notRunningMessage)
                {
                    _notRunningMessage = value;
                    NotifyPropertyChanged("NotRunningMessage");
                }
            }
        }

        private GeoCoordinateCollection _routePath;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public GeoCoordinateCollection RoutePath
        {
            get
            {
                return _routePath;
            }
            set
            {
                if (value != _routePath)
                {
                    _routePath = value;
                    NotifyPropertyChanged("RoutePath");
                }
            }
        }

        private ObservableCollection<RouteStopViewModel> _busAStops;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<RouteStopViewModel> BusAStops
        {
            get
            {
                return _busAStops;
            }
            set
            {
                if (value != _busAStops)
                {
                    _busAStops = value;
                    NotifyPropertyChanged("BusAStops");
                }
            }
        }
        private ObservableCollection<RouteStopViewModel> _busBStops;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<RouteStopViewModel> BusBStops
        {
            get
            {
                return _busBStops;
            }
            set
            {
                if (value != _busBStops)
                {
                    _busBStops = value;
                    NotifyPropertyChanged("BusBStops");
                }
            }
        }

        private RouteStopViewModel _nextAStop;
        public RouteStopViewModel NextAStop
        {
            get
            {
                return _nextAStop;
            }
            set
            {
                if (value != _nextAStop)
                {
                    _nextAStop = value;
                    NotifyPropertyChanged("NextAStop");
                }
            }
        }
        private RouteStopViewModel _nextBStop;
        public RouteStopViewModel NextBStop
        {
            get
            {
                return _nextBStop;
            }
            set
            {
                if (value != _nextBStop)
                {
                    _nextBStop = value;
                    NotifyPropertyChanged("NextBStop");
                }
            }
        }

        public void setNextStop()
        {
            if (BusAStops.Count > 0)
            {
                foreach (RouteStopViewModel routeStop in BusAStops)
                {
                    if (routeStop.TimeUntil.Ticks > 0)
                    {
                        NextAStop = routeStop;
                        break;
                    }
                }
            }
            if (BusBStops.Count > 0)
            {
                foreach (RouteStopViewModel routeStop in BusBStops)
                {
                    if (routeStop.TimeUntil.Ticks > 0)
                    {
                        NextBStop = routeStop;
                        break;
                    }
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