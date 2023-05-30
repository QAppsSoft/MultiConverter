using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers;

public sealed class AudioChannelsGenerator : OptionGeneratorBase
{
    public override string Caption { get; init; } = "Audio Channels";
    public override IOption Generate() => new AudioChannelsOption(AudioChannelsOption.Default);
}
