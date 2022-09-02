namespace MultiConverter.Models.Media;

/// <summary>
///     Represent a subtitle stream
/// </summary>
public interface ISubtitleStream : ILocalSource
{
    /// <summary>
    ///     Can subtitle be burned in video stream
    /// </summary>
    bool CanBurnIn { get; }

    /// <summary>
    ///     Is default subtitle
    /// </summary>
    bool IsDefault { get; }

    /// <summary>
    ///     Subtitle is forced
    /// </summary>
    bool IsForced { get; }

    /// <summary>
    ///     Subtitle language
    /// </summary>
    string Language { get; }

    /// <summary>
    ///     Subtitle language code
    /// </summary>
    string LanguageCode { get; }

    /// <summary>
    ///     Subtitle type
    /// </summary>
    SubtitleType SubtitleType { get; }

    /// <summary>
    ///     Subtitle title
    /// </summary>
    string Title { get; }

    /// <summary>
    ///     True if the subtitle is an external independent file, false other wise
    /// </summary>
    public bool IsExternalSubtitle { get; }
}
