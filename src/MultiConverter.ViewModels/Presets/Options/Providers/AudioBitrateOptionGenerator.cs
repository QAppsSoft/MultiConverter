using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers;

public sealed class AudioBitrateOptionGenerator : OptionGeneratorBase
{
    public override string Caption { get; init; } = "Audio bitrate";
    public override IOption Generate() => new AudioBitrateOption();
}
