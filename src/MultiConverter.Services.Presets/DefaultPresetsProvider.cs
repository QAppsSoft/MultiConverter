using MultiConverter.Models.Presets;
using MultiConverter.Models.Presets.Builder;
using MultiConverter.Models.Presets.Options;
using MultiConverter.Models.PresetsProvider;
using MultiConverter.Services.Abstractions.Presets;

namespace MultiConverter.Services.Presets;

public class DefaultPresetsProvider : IDefaultPresetsProvider
{
    private const string GeneralGroup = "General";

    public IEnumerable<PresetsProviderItem> DefaultPresets { get; } = InitializePresets();

    private static IEnumerable<PresetsProviderItem> InitializePresets() =>
        new List<PresetsProviderItem>
        {
            new(".avi - Xvid 640x480", GeneralGroup, BuildXvid()),
            new(".mp4 - h264 1280x720", GeneralGroup, BuildMpeg4()),
        };

    private static Preset BuildXvid() =>
        PresetBuilder.Configure()
            .WithName(string.Empty)
            .WithFormat("avi")
            .IsDefault(false)
            .IsAdvanced(false)
            .AddOptions(options => options
                .With(new VideoSizeOption(new VideoSize(640, 480)))
                .With(new VideoCodecOption("libxvid"))
                .With(new VideoBitrateOption(1150))
                .With(new VideoAspectRatioOption("4:3"))
                .With(new VideoFrameRateOption(VideoFrameRateOption.Default))
                .With(new AudioCodecOption(AudioCodecOption.Default))
                .With(new AudioBitrateOption(AudioBitrateOption.Default))
                .With(new AudioSamplingRateOption(AudioSamplingRateOption.Default))
                .With(new AudioChannelsOption(AudioChannelsOption.Default)))
            .Build();

    private static Preset BuildMpeg4() =>
        PresetBuilder.Configure()
            .WithName(string.Empty)
            .WithFormat("mp4")
            .IsDefault(false)
            .IsAdvanced(false)
            .AddOptions(options => options
                .With(new VideoSizeOption(new VideoSize(1280, 720)))
                .With(new VideoCodecOption("h264"))
                .With(new VideoBitrateOption(2143))
                .With(new VideoAspectRatioOption("16:9"))
                .With(new VideoFrameRateOption(VideoFrameRateOption.Default))
                .With(new AudioCodecOption("aac"))
                .With(new AudioBitrateOption(160))
                .With(new AudioSamplingRateOption(48000))
                .With(new AudioChannelsOption(AudioChannelsOption.Default)))
            .Build();
}
