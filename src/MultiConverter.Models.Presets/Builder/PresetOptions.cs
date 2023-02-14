using MultiConverter.Models.Presets.Base;

namespace MultiConverter.Models.Presets.Builder;

public sealed class PresetOptions
{
    internal List<OptionBase> Options { get; } = new();

    internal PresetOptions() { }

    public PresetOptions With(OptionBase option)
    {
        Options.Add(option);
        return this;
    }
}
