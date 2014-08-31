using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace slobus_v1._0_db.Converters
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value.GetType() == typeof(DateTime))
            {
                return ((DateTime)value).ToShortTimeString();
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(DateTime) && value.GetType() == typeof(string))
            {
                DateTime newDate;

                if (DateTime.TryParse((string)value, out newDate))
                {
                    return newDate;
                }
            }

            return value;
        }
    }
}
