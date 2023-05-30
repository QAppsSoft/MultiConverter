using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets.Options;

public record AudioChannelsOption(int Channels) : OptionBase
{
    public override IArgument GetArgument() => new CustomArgument($"-ac {Channels}");

    public static int Default { get; } = 2;
}
