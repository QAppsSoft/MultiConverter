using FFMpegCore;
using MultiConverter.Models.Presets.Enums;
using MultiConverter.Models.Presets.Formats;
using MultiConverter.Services.Abstractions.Formats;

namespace MultiConverter.Services.Formats;

public class CodecsProvider : ICodecsProvider
{
    private readonly Lazy<IEnumerable<Codec>> _codecs;

    public CodecsProvider()
    {
        _codecs = new Lazy<IEnumerable<Codec>>(CodecsFactory);
    }

    private static IEnumerable<Codec> CodecsFactory()
    {
        var ffmpegCodecs = FFMpeg.GetCodecs();
        return ffmpegCodecs.Select(GenerateCodec).OrderBy(x => x.Name);
    }

    public IEnumerable<Codec> GetCodecs() => _codecs.Value;

    public IEnumerable<Codec> GetAudioCodecs() => _codecs.Value.Where(codec => codec is { Type: CodecType.Audio, EncodingSupported: true });

    public IEnumerable<Codec> GetVideoCodecs() => _codecs.Value.Where(codec => codec is { Type: CodecType.Video, EncodingSupported: true });

    private static Codec GenerateCodec(FFMpegCore.Enums.Codec codec)
    {
        int positionOfNewLine = codec.Description.IndexOf("\r\n", StringComparison.Ordinal);

        string description = positionOfNewLine > 0 ? codec.Description.Substring(0, positionOfNewLine) : codec.Description;

        return new Codec(
            codec.Name,
            MapType(codec.Type),
            codec.DecodingSupported,
            codec.EncodingSupported,
            description);
    }

    private static CodecType MapType(FFMpegCore.Enums.CodecType codecType) =>
        codecType switch
        {
            FFMpegCore.Enums.CodecType.Unknown => CodecType.Unknown,
            FFMpegCore.Enums.CodecType.Video => CodecType.Video,
            FFMpegCore.Enums.CodecType.Audio => CodecType.Audio,
            FFMpegCore.Enums.CodecType.Subtitle => CodecType.Subtitle,
            FFMpegCore.Enums.CodecType.Data => CodecType.Data,
            _ => throw new ArgumentOutOfRangeException(nameof(codecType), codecType, null)
        };
}
