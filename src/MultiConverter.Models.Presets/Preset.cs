using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets;

public record Preset(string Name, bool Default, VideoFilter[] VideoFilter, AudioFilter[] AudioFilter, OptionBase[] Options);
