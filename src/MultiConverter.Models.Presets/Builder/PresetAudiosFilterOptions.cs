namespace MultiConverter.Models.Presets.Builder;

public sealed class PresetAudiosFilterOptions
{
    internal List<AudioFilter> AudioFilters { get; } = new();

    internal PresetAudiosFilterOptions() { }

    public PresetAudiosFilterOptions With(AudioFilter audioFilter)
    {
        AudioFilters.Add(audioFilter);
        return this;
    }
}
