using System.Collections.Generic;
using MultiConverter.Models.Presets.Formats;

namespace MultiConverter.Services.Abstractions.Formats;

public interface ICodecsProvider
{
    IEnumerable<MediaCodec> GetCodecs();
    IEnumerable<MediaCodec> GetAudioCodecs();
    IEnumerable<MediaCodec> GetVideoCodecs();
}
