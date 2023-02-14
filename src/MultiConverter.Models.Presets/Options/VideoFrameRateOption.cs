using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets.Options;

public record VideoFrameRateOption(double FrameRate) : OptionBase
{
    public override IArgument GetArgument() => new FrameRateArgument(FrameRate);
}
