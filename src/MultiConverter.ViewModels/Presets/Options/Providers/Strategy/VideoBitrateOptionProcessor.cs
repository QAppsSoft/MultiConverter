using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers.Strategy;

public sealed class VideoBitrateOptionProcessor : OptionProcessorBase<VideoBitrateOption>
{
    public override OptionGeneratorBase Generate() => new VideoBitrateOptionGenerator();
}
