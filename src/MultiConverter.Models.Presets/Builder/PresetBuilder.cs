using MultiConverter.Models.Presets.Base;
using MultiConverter.Models.Presets.Output;
using MultiConverter.Models.Presets.Subtitles;

namespace MultiConverter.Models.Presets.Builder;

public sealed class PresetBuilder
{
    private bool _isAdvanced;
    private string _name;
    private bool _defaultPreset;
    private string _format;
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

    public PresetBuilder IsAdvanced(bool advanced = true)
    {
        _isAdvanced = advanced;
        return this;
    }

    public PresetBuilder IsDefault(bool defaultPreset = true)
    {
        _defaultPreset = defaultPreset;
        return this;
    }

    public PresetBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public PresetBuilder WithFormat(string format)
    {
        _format = format;
        return this;
    }

    public Preset Build()
    {
        return new Preset(
            _name,
            _defaultPreset,
            VideoFilters.ToArray(),
            AudioFilters.ToArray(),
            Options.ToArray(),
            _isAdvanced,
            _format,
            InputPostConversion.Default,
            SubtitleStyle.Default,
            OutputPathTemplate.Default
        );
    }
}
