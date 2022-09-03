using System;
using System.Collections.Generic;

namespace MultiConverter.Models.Media;

/// <summary>
///     Represent a container file
/// </summary>
public interface IContainer
{
    /// <summary>
    ///     Gets a Collection of audio tracks associated with this Container
    /// </summary>
    IEnumerable<IAudioStream> AudioTracks { get; }

    /// <summary>
    ///     Gets or sets the length in time of this Container
    /// </summary>
    TimeSpan Duration { get; }

    /// <summary>
    ///     Gets or sets the source file.
    /// </summary>
    string Source { get; }

    /// <summary>
    ///     Gets a Collection of subtitles associated with this Container
    /// </summary>
    IEnumerable<ISubtitleStream> Subtitles { get; }

    /// <summary>
    ///     Gets the video associated with this Container
    /// </summary>
    IVideoStream? Video { get; }
}
