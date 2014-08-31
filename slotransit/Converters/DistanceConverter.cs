using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace slobus_v1._0_db.Converters
{
    public class DistanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value.GetType() == typeof(Double))
            {
                Double num = (Double)value;
                if (Double.IsNaN(num))
                {
                    return "-.-- mi";
                }
                else
                {
                    if (num < 10)
                        return num.ToString("F") + " mi";
                    else if (num < 100)
                        return num.ToString("F1") + " mi";
                    else
                        return num.ToString("F0") + " mi";
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Double) && value.GetType() == typeof(string))
            {
                return Double.Parse(((string)value).Substring(0, 4));
            }

            return value;
        }
    }
}
