using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers.Strategy;

public sealed class VideoSizeOptionProcessor : OptionProcessorBase<VideoSizeOption>
{
    public override OptionGeneratorBase Generate() => new VideoSizeOptionGenerator();
}
