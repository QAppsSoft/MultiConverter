using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets.Options;

public record AudioSamplingRateOption(int SamplingRate = 44100) : OptionBase
{
    public override IArgument GetArgument() => new AudioSamplingRateArgument(SamplingRate);
}
