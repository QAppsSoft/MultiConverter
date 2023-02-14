using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets.Options;

public record VideoBitrateOption(int Bitrate) : OptionBase
{
    public override IArgument GetArgument() => new VideoBitrateArgument(Bitrate);
}
