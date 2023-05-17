using System.Globalization;
using Avalonia.Media;
using MultiConverter.Models.Presets.Subtitles;

namespace MultiConverter.Extension;

public static class AssColorExtensions
{
    public static Color ToColor(this AssColor color) => Color.FromArgb(
        (byte)int.Parse(color.Alpha, NumberStyles.HexNumber),
        (byte)int.Parse(color.Red, NumberStyles.HexNumber),
        (byte)int.Parse(color.Green, NumberStyles.HexNumber),
        (byte)int.Parse(color.Blue, NumberStyles.HexNumber)
    );

    public static AssColor ToAssColor(this Color color)
    {
        return new AssColor(
            color.A.ToString("X2"),
            color.R.ToString("X2"),
            color.G.ToString("X2"),
            color.B.ToString("X2")
        );
    }
}
