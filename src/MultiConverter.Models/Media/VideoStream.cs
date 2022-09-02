namespace MultiConverter.Models.Media;

public readonly record struct VideoStream(int Index, int StreamIndex, string Source, Cropping AutoCropDimensions,
    string Format, double FrameRate, Size Size) : IVideoStream;
