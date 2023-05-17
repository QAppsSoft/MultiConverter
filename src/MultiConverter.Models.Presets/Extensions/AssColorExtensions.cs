using MultiConverter.Models.Presets.Subtitles;

namespace MultiConverter.Models.Presets.Extensions;

public static class AssColorExtensions
{
    public static string ToAssColorString(this AssColor color)
    {
        return $"&H{color.Alpha}{color.Blue}{color.Green}{color.Red}";
    }
}
