using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets.Options;

public record VideoCodecOption(string Codec) : OptionBase
{
    public override IArgument GetArgument() => new VideoCodecArgument(Codec);
    public static string Default => "mpeg4";
}
