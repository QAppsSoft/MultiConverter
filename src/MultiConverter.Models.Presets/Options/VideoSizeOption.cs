using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets.Options;

public record VideoSizeOption(int Width, int Height) : OptionBase
{
    public override IArgument GetArgument() => new SizeArgument(Width, Height);
}
