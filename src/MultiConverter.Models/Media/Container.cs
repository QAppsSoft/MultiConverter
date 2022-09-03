using System;
using System.Collections.Generic;

namespace MultiConverter.Models.Media;

public readonly record struct Container(IEnumerable<IAudioStream> AudioTracks, TimeSpan Duration, string Source,
    IEnumerable<ISubtitleStream> Subtitles, IVideoStream? Video) : IContainer;
