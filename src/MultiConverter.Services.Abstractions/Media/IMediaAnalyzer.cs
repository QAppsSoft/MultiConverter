using System.Threading;
using System.Threading.Tasks;
using MultiConverter.Models.Media;

namespace MultiConverter.Services.Abstractions.Media;

public interface IMediaAnalyzer
{
    /// <summary>
    ///     Get media file metadata
    /// </summary>
    /// <param name="filePath">Path to the media file</param>
    /// <param name="cancellationToken">Analyzer cancellation token</param>
    /// <returns>Media metadata container <see cref="IContainer"/></returns>
    Task<IContainer> AnalyzeAsync(string filePath, CancellationToken? cancellationToken = null);

    /// <summary>
    ///     Get media file metadata
    /// </summary>
    /// <param name="filePath">Path to the media file</param>
    /// <param name="options">Analyzer options</param>
    /// <param name="cancellationToken">Analyzer cancellation token</param>
    /// <returns>Media metadata container <see cref="IContainer"/></returns>
    Task<IContainer> AnalyzeAsync(string filePath, AnalyzerOptions options, CancellationToken? cancellationToken = null);
}
