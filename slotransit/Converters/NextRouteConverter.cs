using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using slobus_v1._0_db.ViewModels;
using slobus_v1._0_db.Resources;

namespace slobus_v1._0_db.Converters
{
    public class NextRouteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return AppResources.NoMoreBusesText;
            }
            
            if (targetType == typeof(string) && value.GetType() == typeof(RouteStopViewModel))
            {
                RouteStopViewModel routeStop = (RouteStopViewModel)value;

                RouteViewModel route = App.ViewModel.Routes[Int32.Parse(routeStop.RouteID)];

                int minutes = routeStop.TimeUntil.Minutes;
                int hours = routeStop.TimeUntil.Hours;

                // Take any remainder of seconds and round up
                if (routeStop.TimeUntil.Seconds > 0)
                    minutes++;

                // Check to see if rounded to 60 minutes
                if (minutes >= 60)
                {
                    hours++;
                    minutes -= 60;
                }

                return "Next Bus: " + hours + ":" + minutes.ToString("D2") +" - " + route.Name;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(RouteStopViewModel) && value.GetType() == typeof(string))
            {
                foreach (StopViewModel stop in App.ViewModel.Stops)
                {
                    if (stop.Name == (string)value)
                        return stop.StopID;
                }
            }

            return value;
        }
    }
}
