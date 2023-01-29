using Avalonia.Data.Converters;
using Avalonia.Media;
using MultiConverter.Extension;

namespace MultiConverter.Views.Converters;

public static class GeometryConverters
{
    /// <summary>
    ///     A value converter that returns an icon if the input bool is true or false, representing an Edit state
    /// </summary>
    public static IValueConverter ResourceKeyToGeometry =>
        new FuncValueConverter<string, StreamGeometry?>(key => key.GetResource<StreamGeometry>());
}
