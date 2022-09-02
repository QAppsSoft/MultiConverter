namespace MultiConverter.Models.Media;

/// <summary>
///     Represent an stream of data in a video container file
/// </summary>
public interface IStream
{
    /// <summary>
    ///     Index number of the stream in the container
    /// </summary>
    int Index { get; }

    /// <summary>
    ///     Index number for the specific stream type
    /// </summary>
    int StreamIndex { get; }
}
