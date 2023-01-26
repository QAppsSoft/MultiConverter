using System;
using System.Globalization;
using Avalonia.Data;
using Material.Icons;

namespace MultiConverter.Views.Converters;

public class EditButtonStatusToSymbolEnumConverter : ConvertBase
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isEditing)
        {
            return isEditing ? MaterialIconKind.ContentSave : MaterialIconKind.Edit;
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
}
