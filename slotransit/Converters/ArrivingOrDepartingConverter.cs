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
    public class ArrivingOrDepartingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value.GetType() == typeof(string))
            {

                if (((string)value).EndsWith("Arrive") || ((string)value).EndsWith("Depart"))
                {
                    string[] subs = ((string)value).Split('?');
                    switch (subs[1])
                    {
                        case "Arrive":
                            return "arriving";
                        case "Depart":
                            return "departing";
                        default:
                            return subs[1];
                    }
                }
                else
                {
                    return "";
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && value.GetType() == typeof(string))
            {

                switch (value as string)
                {
                    case "arriving":
                        return "?Arrive";
                    case "departing":
                        return "?Depart";
                    default:
                        return "?" + (string)value;
                }
            }

            return value;
        }
    }
}
