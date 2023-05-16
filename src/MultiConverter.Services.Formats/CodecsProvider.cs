using FFMpegCore;
using MultiConverter.Models.Configurations;
using MultiConverter.Models.Presets.Enums;
using MultiConverter.Models.Presets.Formats;
using MultiConverter.Services.Abstractions.Formats;

namespace MultiConverter.Services.Formats;

public class CodecsProvider : ICodecsProvider
{
    private readonly FavoriteCodecsConfiguration _configuration;
    private readonly Lazy<IEnumerable<Codec>> _codecs;

    public CodecsProvider(FavoriteCodecsConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _configuration = configuration;

        _codecs = new Lazy<IEnumerable<Codec>>(CodecsFactory);
    }

    private IEnumerable<Codec> CodecsFactory()
    {
        var ffmpegCodecs = FFMpeg.GetCodecs();
        return ffmpegCodecs.Select(GenerateCodec)
            .OrderByDescending(x => x.Favorite)
            .ThenBy(x => x.Name);
    }

    public IEnumerable<Codec> GetCodecs() => _codecs.Value;

    public IEnumerable<Codec> GetAudioCodecs() => GetCodecs().Where(codec => codec is { Type: CodecType.Audio, EncodingSupported: true });

    public IEnumerable<Codec> GetVideoCodecs() => GetCodecs().Where(codec => codec is { Type: CodecType.Video, EncodingSupported: true });

    private Codec GenerateCodec(FFMpegCore.Enums.Codec codec)
    {
        int positionOfNewLine = codec.Description.IndexOf("\r\n", StringComparison.Ordinal);

        string description = positionOfNewLine > 0 ? codec.Description.Substring(0, positionOfNewLine) : codec.Description;
        bool favorite = _configuration.Favorites.Contains(codec.Name);

        return new Codec(
            codec.Name,
            MapType(codec.Type),
            codec.DecodingSupported,
            codec.EncodingSupported,
            description,
            favorite);
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
