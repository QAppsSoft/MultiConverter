using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace MultiConverter.Views.Converters;

public class ConvertBase : IValueConverter
{
    public virtual object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        AvaloniaProperty.UnsetValue;

    public virtual object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        AvaloniaProperty.UnsetValue;
}
