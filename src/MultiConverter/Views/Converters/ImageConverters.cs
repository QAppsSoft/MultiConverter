using Avalonia.Data.Converters;
using Avalonia.Media;
using MultiConverter.Extension;

namespace MultiConverter.Views.Converters;

public static class ImageConverters
{
    /// <summary>
    ///     A value converter that returns an icon if the input bool is true or false, representing an Edit state
    /// </summary>
    public static IValueConverter ResourceKeyToDrawingImage =>
        new FuncValueConverter<string, DrawingImage?>(key => key.GetResource<DrawingImage>());
}
