using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets.Options;

public record VideoFrameRateOption(double FrameRate = 29.97) : OptionBase
{
    public override IArgument GetArgument() => new FrameRateArgument(FrameRate);

    public static double Default { get; } = 29.97;
}
