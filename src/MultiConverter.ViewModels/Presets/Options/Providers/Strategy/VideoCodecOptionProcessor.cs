using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers.Strategy;

public sealed class VideoCodecOptionProcessor : OptionProcessorBase<VideoCodecOption>
{
    public override OptionGeneratorBase Generate() => new VideoCodecOptionGenerator();
}
