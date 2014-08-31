using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Device.Location;

namespace slobus_v1._0_db.ViewModels
{
    public class StopViewModel : INotifyPropertyChanged
    {
        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                if (value != _index)
                {
                    _index = value;
                    NotifyPropertyChanged("Index");
                }
            }
        }
        
        private string _stopId;
        /// <summary>
        /// ViewModel property; this property is used to identify the route.
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

        private GeoCoordinate _location;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public GeoCoordinate Location
        {
            get
            {
                return _location;
            }
            set
            {
                if (value != _location)
                {
                    _location = value;
                    NotifyPropertyChanged("Location");
                }
            }
        }

        private ObservableCollection<RouteStopViewModel> _routeStops;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<RouteStopViewModel> RouteStops
        {
            get
            {
                return _routeStops;
            }
            set
            {
                if (value != _routeStops)
                {
                    _routeStops = value;
                    NotifyPropertyChanged("RouteStops");
                }
            }
        }

        private RouteStopViewModel _nextRoute;
        public RouteStopViewModel NextRoute
        {
            get
            {
                return _nextRoute;
            }
            set
            {
                if (value != _nextRoute)
                {
                    _nextRoute = value;
                    NotifyPropertyChanged("NextRoute");
                }
            }
        }
        private Double _distance = Double.NaN;
        public Double Distance
        {
            get
            {
                return _distance;
            }
            set
            {
                if (value != _distance)
                {
                    _distance = value;
                    NotifyPropertyChanged("Distance");
                }
            }
        }


        public void setNextRoute()
        {
            if (RouteStops.Count > 0)
            {
                foreach (RouteStopViewModel routeStop in RouteStops)
                {
                    if (routeStop.TimeUntil.Ticks > 0)
                    {
                        NextRoute = routeStop;
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