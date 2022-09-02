namespace MultiConverter.Models.Media;

/// <summary>
///     Represent an stream on a physical file
/// </summary>
public interface ILocalSource : IStream
{
    /// <summary>
    ///     Path to the physical file
    /// </summary>
    string Source { get; }
}
