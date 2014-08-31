using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using slobus_v1._0_db.ViewModels;

namespace slobus_v1._0_db.Converters
{
    public class RouteIDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value.GetType() == typeof(string))
            {

                string id = (string)value;


                var match = from route in App.ViewModel.Routes
                            where route.RouteID.Equals(id)
                            select route.Name;
                if(match.Count() > 0)
                {
                return match.First();
                }
                else
                {
                    return value;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value.GetType() == typeof(string))
            {
                foreach (RouteViewModel route in App.ViewModel.Routes)
                {
                    if (route.Name == (string)value)
                        return route.RouteID;
                }
            }

            return value;
        }
    }
}
