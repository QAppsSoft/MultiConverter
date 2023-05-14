using AutoMapper;
using FFMpegCore;
using MultiConverter.Models.Presets.Enums;
using MultiConverter.Models.Presets.Formats;
using MultiConverter.Services.Abstractions.Formats;

namespace MultiConverter.Services.Formats;

public class CodecsProvider : ICodecsProvider
{
    private readonly IMapper _mapper;
    private readonly Lazy<IEnumerable<Codec>> _codecs;

    public CodecsProvider(IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _codecs = new Lazy<IEnumerable<Codec>>(CodecsFactory);
    }

    private IEnumerable<Codec> CodecsFactory()
    {
        var ffmpegCodecs = FFMpeg.GetCodecs();
        return _mapper.Map<IEnumerable<FFMpegCore.Enums.Codec>, IEnumerable<Codec>>(ffmpegCodecs);
    }

    public IEnumerable<Codec> GetCodecs() => _codecs.Value;

    public IEnumerable<Codec> GetAudioCodecs() => _codecs.Value.Where(codec => codec.Type == CodecType.Audio);

    public IEnumerable<Codec> GetVideoCodecs() => _codecs.Value.Where(codec => codec.Type == CodecType.Video);
}
