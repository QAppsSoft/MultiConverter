using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets.Options;

public record AudioCodecOption(string AudioCodec) : OptionBase
{
    public override IArgument GetArgument() => new AudioCodecArgument(AudioCodec);
    public static string Default => "libmp3lame";
}
