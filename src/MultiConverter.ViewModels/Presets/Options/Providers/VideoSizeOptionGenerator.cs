using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers;

public sealed class VideoSizeOptionGenerator : OptionGeneratorBase
{
    public override string Caption { get; init; } = "Video size";
    public override IOption Generate() => new VideoSizeOption(VideoSizeOption.Default);
}
