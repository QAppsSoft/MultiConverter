using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers;

public sealed class VideoFrameRateOptionGenerator : OptionGeneratorBase
{
    public override string Caption { get; init; } = "Video frame rate";
    public override IOption Generate() => new VideoFrameRateOption();
}
