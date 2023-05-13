namespace MultiConverter.Models.Presets.Builder;

public sealed class PresetVideoFilterOptions
{
    internal List<VideoFilter> VideoFilters { get; } = new();

    internal PresetVideoFilterOptions() { }

    public PresetVideoFilterOptions With(VideoFilter videoFilter)
    {
        VideoFilters.Add(videoFilter);
        return this;
    }
}
