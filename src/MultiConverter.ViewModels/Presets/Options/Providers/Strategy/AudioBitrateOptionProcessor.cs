using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers.Strategy;

public sealed class AudioBitrateOptionProcessor : OptionProcessorBase<AudioBitrateOption>
{
    public override OptionGeneratorBase Generate() => new AudioBitrateOptionGenerator();
}
