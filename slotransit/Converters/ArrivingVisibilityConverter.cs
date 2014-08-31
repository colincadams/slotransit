using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace slobus_v1._0_db.Converters
{
    public class ArrivingVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Visibility) && value.GetType() == typeof(string))
            {
                string[] subs = ((string)value).Split('?');
                if (subs.Count() == 2)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;

            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value.GetType() == typeof(Visibility))
            {
                if ((Visibility)value == Visibility.Collapsed)
                    return "? ";
                else
                    return "";
            }

            return value;
        }
    }
}
