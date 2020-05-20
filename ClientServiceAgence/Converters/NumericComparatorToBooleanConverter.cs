using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ClientServiceAgence.Converters
{
    class NumericComparatorToBooleanConverter : IMultiValueConverter
    {
        public enum NumericComparatorToBooleanParameter
        {
            Equal = 0,
            Different = 1,
            Less = 2,
            Greater = 3,
            LessOrEqual = 4,
            GreaterOrEqual = 5
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = true;

            if (values == null || values.Length < 2) return null;

            parameter = GetParameterValue(parameter);
            for (int i = 0; i < values.Length - 1; i++)
            {
                double val1, val2;
                if (!GetNumericValue(values[i], out val1)) return null;
                if (!GetNumericValue(values[i + 1], out val2)) return null;

                switch ((NumericComparatorToBooleanParameter)(parameter))
                {
                    case NumericComparatorToBooleanParameter.Different:
                        result = result && (val1 != val2);
                        break;
                    case NumericComparatorToBooleanParameter.Equal:
                        result = result && (val1 == val2);
                        break;
                    case NumericComparatorToBooleanParameter.Greater:
                        result = result && (val1 > val2);
                        break;
                    case NumericComparatorToBooleanParameter.GreaterOrEqual:
                        result = result && (val1 >= val2);
                        break;
                    case NumericComparatorToBooleanParameter.Less:
                        result = result && (val1 < val2);
                        break;
                    case NumericComparatorToBooleanParameter.LessOrEqual:
                        result = result && (val1 <= val2);
                        break;
                }
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private NumericComparatorToBooleanParameter GetParameterValue(object parameter)
        {
            if (parameter != null)
            {
                return ((NumericComparatorToBooleanParameter)(int.Parse(parameter.ToString())));
            }
            return NumericComparatorToBooleanParameter.Equal;
        }
        private bool GetNumericValue(object value, out double numValue)
        {
            numValue = 0;
            if (value == null) return false;
            try
            {
                return double.TryParse(value.ToString(), out numValue);
            }
            catch { return false; }

        }
    }
}
