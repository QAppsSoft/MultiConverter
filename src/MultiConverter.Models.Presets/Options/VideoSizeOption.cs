using FFMpegCore.Arguments;
using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets.Options;

public record VideoSizeOption(VideoSize VideoSize) : OptionBase
{
    public VideoSizeOption(int width, int height) : this(new VideoSize(width, height))
    {
    }

    public override IArgument GetArgument() => new SizeArgument(VideoSize.Width, VideoSize.Height);

    public static VideoSize Default => VideoSize.Default;
}

public record VideoSize(int Width, int Height)
{
    public static VideoSize Default { get; } = new(1024, 768);
}
