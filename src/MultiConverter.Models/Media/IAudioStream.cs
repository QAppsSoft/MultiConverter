using System;

namespace MultiConverter.Models.Media;

/// <summary>
///     Represent an audio stream
/// </summary>
public interface IAudioStream : ILocalSource
{
    /// <summary>
    ///     Audio bitrate
    /// </summary>
    double BitRate { get; }

    /// <summary>
    ///     Audio duration
    /// </summary>
    TimeSpan Duration { get; }

    /// <summary>
    ///     Audio codec/format
    /// </summary>
    string Format { get; }

    /// <summary>
    ///     Is default audio stream
    /// </summary>
    bool IsDefault { get; }

    /// <summary>
    ///     Is forced
    /// </summary>
    bool IsForced { get; }

    /// <summary>
    ///     Audio language
    /// </summary>
    string Language { get; }

    /// <summary>
    ///     Audio language code
    /// </summary>
    string LanguageCode { get; }

    /// <summary>
    ///     Audio sample rate in b/s
    /// </summary>
    int SampleRate { get; }
}
