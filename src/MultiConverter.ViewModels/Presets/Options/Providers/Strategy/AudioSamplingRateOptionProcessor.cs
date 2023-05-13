using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers.Strategy;

public sealed class AudioSamplingRateOptionProcessor : OptionProcessorBase<AudioSamplingRateOption>
{
    public override OptionGeneratorBase Generate() => new AudioSamplingRateOptionGenerator();
}
