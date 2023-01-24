using System;
using System.Globalization;
using Avalonia.Data;
using Material.Icons;

namespace MultiConverter.Views.Converters;

public class DrawerMenuIconConverter : ConvertBase
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isOpened)
        {
            return isOpened ? MaterialIconKind.ArrowLeft : MaterialIconKind.Menu;
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
}
