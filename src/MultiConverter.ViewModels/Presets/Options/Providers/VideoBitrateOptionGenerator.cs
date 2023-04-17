using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers;

public sealed class VideoBitrateOptionGenerator : OptionGeneratorBase
{
    public override string Caption { get; init; } = "Video bitrate";
    public override IOption Generate() => new VideoBitrateOption(VideoBitrateOption.Default);
}
