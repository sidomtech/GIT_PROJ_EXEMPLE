using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace ClientServiceAgence.Converters
{
    class VisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility result;

            if (parameter != null && parameter.ToString().ToLower() == "or")
            {
                result = Visibility.Collapsed;

                for (int i = 0; i < values.Length - 1; i++)
                {
                    if ((Visibility)values[i] == Visibility.Visible)
                    {
                        result = Visibility.Visible;
                        break;
                    }
                }
            }
            else
            {
                result = Visibility.Visible;

                for (int i = 0; i < values.Length - 1; i++)
                {
                    if ((Visibility)values[i] != Visibility.Visible)
                    {
                        result = Visibility.Collapsed;
                        break;
                    }
                }
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
