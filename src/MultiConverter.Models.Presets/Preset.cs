using MultiConverter.Models.Presets.Interfaces;

namespace MultiConverter.Models.Presets;

public record Preset(string Name, bool IsDefault, VideoFilter[] VideoFilter, AudioFilter[] AudioFilter,
    IOption[] Options)
{
    public static Preset Empty => new(
        string.Empty,
        false,
        Array.Empty<VideoFilter>(),
        Array.Empty<AudioFilter>(),
        Array.Empty<IOption>());
}
