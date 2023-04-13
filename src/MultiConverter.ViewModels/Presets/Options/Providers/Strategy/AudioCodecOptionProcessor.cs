using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers.Strategy;

public sealed class AudioCodecOptionProcessor : OptionProcessorBase<AudioCodecOption>
{
    public override OptionGeneratorBase Generate() => new AudioCodecOptionGenerator();
}
