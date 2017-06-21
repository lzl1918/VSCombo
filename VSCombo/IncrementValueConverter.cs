using System;
using System.Globalization;
using System.Windows.Data;

namespace VSCombo
{
    public sealed class IncrementValueConverter : IValueConverter
    {
        public double Increment { get; set; }
        public IncrementValueConverter()
        {
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v)
            {
                return v + Increment;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
