using MultiConverter.Models.Presets.Interfaces;

namespace MultiConverter.Models.Presets;

public record Preset(string Name, bool IsDefault, VideoFilter[] VideoFilter, AudioFilter[] AudioFilter, IOption[] Options);
