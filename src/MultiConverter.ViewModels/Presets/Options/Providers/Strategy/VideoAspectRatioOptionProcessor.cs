using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers.Strategy;

public sealed class VideoAspectRatioOptionProcessor : OptionProcessorBase<VideoAspectRatioOption>
{
    public override OptionGeneratorBase Generate() => new VideoAspectRatioOptionGenerator();
}
