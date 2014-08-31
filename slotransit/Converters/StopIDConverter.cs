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
    public class StopIDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value.GetType() == typeof(string))
            {
                
                string id;
                
                string[] subs = ((string)value).Split('?');
                id = subs[0];


                var match = from stop in App.ViewModel.Stops
                            where stop.StopID.Equals(id)
                            select stop.Name;
                if (match.Count() > 0)
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
