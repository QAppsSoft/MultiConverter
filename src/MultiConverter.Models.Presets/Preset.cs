using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using MultiConverter.Models.Presets.Output;
using MultiConverter.Models.Presets.Subtitles;

namespace MultiConverter.Models.Presets;

public record Preset(string Name, bool IsDefault, VideoFilter[] VideoFilter, AudioFilter[] AudioFilter,
    IOption[] Options, bool IsAdvanced, string ContainerFormat, InputPostConversion PostConversion,
    SubtitleStyle SubtitleStyle, OutputPathTemplate OutputPathTemplate)
{
    public virtual bool Equals(Preset? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Name == other.Name &&
               IsDefault == other.IsDefault &&
               VideoFilter.SequenceEqual(other.VideoFilter) &&
               AudioFilter.SequenceEqual(other.AudioFilter) &&
               Options.SequenceEqual(other.Options) &&
               IsAdvanced == other.IsAdvanced &&
               ContainerFormat == other.ContainerFormat &&
               PostConversion.Equals(other.PostConversion) &&
               SubtitleStyle.Equals(other.SubtitleStyle) &&
               OutputPathTemplate.Equals(other.OutputPathTemplate);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Name);
        hashCode.Add(IsDefault);
        hashCode.Add(VideoFilter);
        hashCode.Add(AudioFilter);
        hashCode.Add(Options);
        hashCode.Add(IsAdvanced);
        hashCode.Add(ContainerFormat);
        hashCode.Add(PostConversion);
        hashCode.Add(SubtitleStyle);
        hashCode.Add(OutputPathTemplate);
        return hashCode.ToHashCode();
    }

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
