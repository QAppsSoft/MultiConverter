using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using MultiConverter.Models.Presets.Output;
using MultiConverter.Models.Presets.Subtitles;

namespace MultiConverter.Models.Presets;

public record Preset(string Name, bool IsDefault, VideoFilter[] VideoFilter, AudioFilter[] AudioFilter,
    IOption[] Options, bool IsAdvanced, string ContainerFormat, InputPostConversion PostConversion,
    SubtitleStyle SubtitleStyle, OutputPathTemplate OutputPathTemplate)
{
    private static IOption[] GetBasicOptions { get; } = {
        new VideoSizeOption(VideoSize.Default),
        new VideoCodecOption(VideoCodecOption.Default),
        new VideoBitrateOption(VideoBitrateOption.Default),
        new VideoAspectRatioOption(VideoAspectRatioOption.Default),
        new VideoFrameRateOption(),

        new AudioCodecOption(AudioCodecOption.Default),
        new AudioBitrateOption(),
        new AudioSamplingRateOption(),
        new AudioChannelsOption(2)
    };

    public static Preset Empty { get; } = new(
        string.Empty,
        false,
        Array.Empty<VideoFilter>(),
        Array.Empty<AudioFilter>(),
        GetBasicOptions,
        false,
        "matroska",
        InputPostConversion.Default,
        SubtitleStyle.Default, OutputPathTemplate.Default
    );
}
