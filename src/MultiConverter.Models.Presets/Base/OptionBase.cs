using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Interfaces;

namespace MultiConverter.Models.Presets.Base;

public abstract record OptionBase : IOption
{
    public abstract IArgument GetArgument();
}
