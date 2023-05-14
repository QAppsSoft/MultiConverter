using MultiConverter.Models.Presets.Enums;

namespace MultiConverter.Models.Presets.Formats;

public record MediaCodec(string Name, CodecType Type, bool DecodingSupported, bool EncodingSupported, string Description);
