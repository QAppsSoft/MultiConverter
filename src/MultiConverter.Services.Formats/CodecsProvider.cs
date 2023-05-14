using AutoMapper;
using FFMpegCore;
using MultiConverter.Models.Presets.Enums;
using MultiConverter.Models.Presets.Formats;
using MultiConverter.Services.Abstractions.Formats;

namespace MultiConverter.Services.Formats;

public class CodecsProvider : ICodecsProvider
{
    private readonly IMapper _mapper;
    private readonly Lazy<IEnumerable<MediaCodec>> _codecs;

    public CodecsProvider(IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _codecs = new Lazy<IEnumerable<MediaCodec>>(CodecsFactory);
    }

    private IEnumerable<MediaCodec> CodecsFactory()
    {
        var ffmpegCodecs = FFMpeg.GetCodecs();
        return _mapper.Map<IEnumerable<FFMpegCore.Enums.Codec>, IEnumerable<MediaCodec>>(ffmpegCodecs);
    }

    public IEnumerable<MediaCodec> GetCodecs() => _codecs.Value;

    public IEnumerable<MediaCodec> GetAudioCodecs() => _codecs.Value.Where(codec => codec.Type == CodecType.Audio);

    public IEnumerable<MediaCodec> GetVideoCodecs() => _codecs.Value.Where(codec => codec.Type == CodecType.Video);
}
