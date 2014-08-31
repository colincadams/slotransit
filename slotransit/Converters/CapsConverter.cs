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
    public class CapsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value.GetType() == typeof(string))
            {

                return (value as string).ToUpper();
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value.GetType() == typeof(string))
            {

                return (value as string).ToLower();
            }

            return value;
        }
    }
}
