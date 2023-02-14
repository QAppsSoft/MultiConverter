using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets;

public abstract record VideoFilter : FilterBase
{
    public abstract IVideoFilterArgument GetFilter();
}
