using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers.Strategy;

public sealed class VideoFrameRateOptionProcessor : OptionProcessorBase<VideoFrameRateOption>
{
    public override OptionGeneratorBase Generate() => new VideoFrameRateOptionGenerator();
}
