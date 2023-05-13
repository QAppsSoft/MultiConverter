using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers;

public sealed class VideoCodecOptionGenerator : OptionGeneratorBase
{
    public override string Caption { get; init; } = "Video codec";
    public override IOption Generate() => new VideoCodecOption(VideoCodecOption.Default);
}
