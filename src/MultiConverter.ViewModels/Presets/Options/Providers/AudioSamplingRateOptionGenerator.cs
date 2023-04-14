using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers;

public sealed class AudioSamplingRateOptionGenerator : OptionGeneratorBase
{
    public override string Caption { get; init; } = "Audio sampling rate";
    public override IOption Generate() => new AudioSamplingRateOption();
}
