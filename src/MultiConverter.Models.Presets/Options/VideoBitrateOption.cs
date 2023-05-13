using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets.Options;

public record VideoBitrateOption(int Bitrate) : OptionBase
{
    public override IArgument GetArgument() => new VideoBitrateArgument(Bitrate);
    public static int Default => 650; // 650 kB
}
