namespace MultiConverter.Models.Media;

public interface IVideoStream : ILocalSource
{
    /// <summary>
    ///     Gets or sets the automatically detected crop region for this Container.
    /// </summary>
    Cropping AutoCropDimensions { get; }

    string Format { get; }

    /// <summary>
    ///     Gets or sets the frame rate of the source.
    /// </summary>
    double FrameRate { get; }

    /// <summary>
    ///     Gets or sets the size (width/height) of this Container
    /// </summary>
    Size Size { get; }
}
