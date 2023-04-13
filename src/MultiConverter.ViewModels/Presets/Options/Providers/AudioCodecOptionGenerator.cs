using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers;

public sealed class AudioCodecOptionGenerator : OptionGeneratorBase
{
    public override string Caption { get; init; } = "Audio codec";
    public override IOption Generate() => new AudioCodecOption(AudioCodecOption.Default);
}
