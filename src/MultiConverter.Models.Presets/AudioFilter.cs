using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets;

public abstract record AudioFilter : FilterBase
{
    public abstract IAudioFilterArgument GetFilter();
}
