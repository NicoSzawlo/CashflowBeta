using System;
using System.Globalization;

namespace CashflowBeta.Converters;

public interface ICustomValueConverter
{
    object Convert(object value, Type targetType, object parameter, CultureInfo culture);
    object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
}