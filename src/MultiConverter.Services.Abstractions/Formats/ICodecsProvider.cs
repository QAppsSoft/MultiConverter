using System.Collections.Generic;
using MultiConverter.Models.Presets.Formats;

namespace MultiConverter.Services.Abstractions.Formats;

public interface ICodecsProvider
{
    IEnumerable<Codec> GetCodecs();
    IEnumerable<Codec> GetAudioCodecs();
    IEnumerable<Codec> GetVideoCodecs();
}
