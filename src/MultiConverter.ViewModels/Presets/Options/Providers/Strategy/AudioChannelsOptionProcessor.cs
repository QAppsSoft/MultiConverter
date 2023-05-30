using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers.Strategy;

public sealed class AudioChannelsOptionProcessor : OptionProcessorBase<AudioChannelsOption>
{
    public override OptionGeneratorBase Generate() => new AudioChannelsGenerator();
}
