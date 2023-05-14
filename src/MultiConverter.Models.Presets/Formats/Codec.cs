using MultiConverter.Models.Presets.Enums;

namespace MultiConverter.Models.Presets.Formats;

public record Codec(string Name, CodecType Type, bool DecodingSupported, bool EncodingSupported, string Description);
