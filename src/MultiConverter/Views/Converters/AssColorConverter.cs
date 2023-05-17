using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Media;
using MultiConverter.Extension;
using MultiConverter.Models.Presets.Subtitles;

namespace MultiConverter.Views.Converters;

public class AssColorConverter : ConvertBase
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        switch (value)
        {
            case null:
                return null;
            case Color color:
                return color.ToAssColor();
            case AssColor assColor:
                return assColor.ToColor();
            default:
                {
                    string message = $"Could not convert '{value}' to '{targetType.Name}'.";
                    return new BindingNotification(new InvalidCastException(message), BindingErrorType.Error);
                }
        }
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        Convert(value, targetType, parameter, culture);
}
