using FFMpegCore.Arguments;

namespace MultiConverter.Models.Presets.Interfaces;

public interface IOption
{
    IArgument GetArgument();
}
