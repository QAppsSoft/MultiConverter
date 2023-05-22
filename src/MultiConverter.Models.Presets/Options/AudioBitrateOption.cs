using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets.Options;

public record AudioBitrateOption(int Bitrate = 128) : OptionBase
{
    public override IArgument GetArgument() => new AudioBitrateArgument(Bitrate);

    public static int Default { get; } = 128;
}
