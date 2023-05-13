using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets.Builder;

public sealed class PresetBuilder
{
    private List<OptionBase> Options { get; } = new();
    private List<AudioFilter> AudioFilters { get; } = new();
    private List<VideoFilter> VideoFilters { get; } = new();

    private PresetBuilder()
    {
    }

    public static PresetBuilder Configure() => new();

    public PresetBuilder AddOptions(Action<PresetOptions> options)
    {
        PresetOptions presetOptions = new();
        options.Invoke(presetOptions);

        foreach (OptionBase option in presetOptions.Options)
        {
            Options.Add(option);
        }

        return this;
    }

    public PresetBuilder AddVideoFilter(Action<PresetVideoFilterOptions> options)
    {
        PresetVideoFilterOptions presetOptions = new();
        options.Invoke(presetOptions);

        foreach (VideoFilter filter in presetOptions.VideoFilters)
        {
            VideoFilters.Add(filter);
        }

        return this;
    }

    public PresetBuilder AddAudioFilter(Action<PresetAudiosFilterOptions> options)
    {
        PresetAudiosFilterOptions presetOptions = new();
        options.Invoke(presetOptions);

        foreach (AudioFilter filter in presetOptions.AudioFilters)
        {
            AudioFilters.Add(filter);
        }

        return this;
    }

    public Preset Build()
    {
        return new Preset(string.Empty, false, VideoFilters.ToArray(), AudioFilters.ToArray(), Options.ToArray(),
            false);
    }
}
