using Avalonia.Data.Converters;
using Material.Icons;

namespace MultiConverter.Views.Converters;

public static class SymbolConverters
{
    /// <summary>
    ///     A value converter that returns an icon if the input bool is true or false, representing an Edit state
    /// </summary>
    public static readonly IValueConverter EditStatusToSymbol =
        new FuncValueConverter<bool, MaterialIconKind>(value =>
            value ? MaterialIconKind.ContentSave : MaterialIconKind.Edit);
}

