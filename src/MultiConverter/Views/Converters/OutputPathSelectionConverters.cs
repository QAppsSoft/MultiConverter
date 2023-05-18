using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using MultiConverter.Models.Presets.Enums;

namespace MultiConverter.Views.Converters;

public static class OutputPathSelectionConverter
{
    public static IValueConverter IsVisibleIf => new OutputPathSelectionToBoolConverter();
}

public class OutputPathSelectionToBoolConverter : ConvertBase
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        switch (value)
        {
            case null:
                return null;
            case OutputPathSelection pathSelection when parameter is OutputPathSelection parameterValue:
                return pathSelection == parameterValue;
            default:
                {
                    string message = $"Could not convert '{value}' to '{targetType.Name}' with parameter '{parameter}'.";
                    return new BindingNotification(new InvalidCastException(message), BindingErrorType.Error);
                }
        }
    }
}
