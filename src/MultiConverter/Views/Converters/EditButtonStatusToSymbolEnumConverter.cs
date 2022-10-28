using System;
using System.Globalization;
using Avalonia.Data;
using FluentAvalonia.UI.Controls;

namespace MultiConverter.Views.Converters;

public class EditButtonStatusToSymbolEnumConverter : ConvertBase
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isEditing)
        {
            return isEditing ? Symbol.Save : Symbol.Edit;
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
}
