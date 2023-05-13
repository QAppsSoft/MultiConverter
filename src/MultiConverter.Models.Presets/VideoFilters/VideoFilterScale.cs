using FFMpegCore.Arguments;

namespace MultiConverter.Models.Presets.VideoFilters;

public record VideoFilterScale(int Width, int Height) : VideoFilter
{
    public override IVideoFilterArgument GetFilter() => new ScaleArgument(Width, Height);
}
