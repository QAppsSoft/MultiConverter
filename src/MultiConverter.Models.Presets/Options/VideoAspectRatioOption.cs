using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets.Options;

public record VideoAspectRatioOption(string AspectRatio) : OptionBase
{
    public override IArgument GetArgument() => new CustomArgument($"-aspect {AspectRatio}");

    public static string Default => "16:9";
}
