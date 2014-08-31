using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace slobus_v1._0_db.Converters
{
    public class TimeUntilConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value.GetType() == typeof(TimeSpan))
            {
                TimeSpan TimeUntil = (TimeSpan)value;

                int minutes = TimeUntil.Minutes;
                int hours = TimeUntil.Hours;

                // Take any remainder of seconds and round up
                if (TimeUntil.Seconds > 0)
                    minutes++;

                // Check to see if rounded up to 60
                if (minutes >= 60)
                {
                    hours++;
                    minutes -= 60;
                }

                if (hours > 0)
                    return hours + " h " + minutes + " min";
                if (minutes > 0)
                    return minutes + " min";
                return "";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(TimeSpan) && value.GetType() == typeof(string))
            {
                string text = (string)value;

                int[] arr = text.Split(' ').Select(p => int.Parse(p)).ToArray();

                int hours = 0;
                int minutes = 0;

                if (arr.Count() == 2)
                {
                    hours = arr[0];
                    minutes = arr[1];
                }
                else if (arr.Count() == 1)
                    minutes = arr[0];

                return new TimeSpan(hours, minutes, 0);
            }

            return value;
        }
    }
}
