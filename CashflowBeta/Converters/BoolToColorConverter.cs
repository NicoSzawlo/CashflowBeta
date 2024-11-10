using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace CashflowBeta.Converters;

public class BoolToColorConverter : ICustomValueConverter, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        
        if (value is bool isValid)
        {
            return isValid ? Brushes.Transparent : Brushes.Red; // Change to desired color
        }   
        return Brushes.Red;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}