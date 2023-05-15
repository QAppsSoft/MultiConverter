using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;

namespace MultiConverter.Models.Presets;

public record Preset(string Name, bool IsDefault, VideoFilter[] VideoFilter, AudioFilter[] AudioFilter,
    IOption[] Options, bool IsAdvanced, string ContainerFormat)
{
    private static IOption[] GetBasicOptions { get; } = {
        new VideoSizeOption(VideoSize.Default),
        new VideoCodecOption(VideoCodecOption.Default),
        new VideoBitrateOption(VideoBitrateOption.Default),
        new VideoAspectRatioOption(VideoAspectRatioOption.Default),
        new VideoFrameRateOption(),

        new AudioCodecOption(AudioCodecOption.Default),
        new AudioBitrateOption(),
        new AudioSamplingRateOption()
    };

    public static Preset Empty { get; } = new(
        string.Empty,
        false,
        Array.Empty<VideoFilter>(),
        Array.Empty<AudioFilter>(),
        GetBasicOptions,
        false,
        "matroska");
}
