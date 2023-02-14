using System;
using System.Text.Json;
using MultiConverter.Models.Presets;
using MultiConverter.Services.Abstractions.Settings;

namespace MultiConverter.Services.Settings.Presets;

public class PresetsConverter : IConverter<Preset[]>
{
    private static JsonSerializerOptions SerializerOptions { get; } =
        new() { WriteIndented = true, TypeInfoResolver = new PresetJsonTypeResolver(), };

    public Preset[] Convert(State state)
    {
        Preset[] defaults = GetDefaultValue();

        if (state == State.Empty)
        {
            return defaults;
        }

        Preset[]? presets = JsonSerializer.Deserialize<Preset[]>(state.Value, SerializerOptions);

        if (presets == default)
        {
            return defaults;
        }

        return state.Version switch
        {
            1 => presets,
            _ => defaults
        };
    }

    public State Convert(Preset[] state)
    {
        return new State(1, JsonSerializer.Serialize(state, SerializerOptions));
    }

    public Preset[] GetDefaultValue() => Array.Empty<Preset>();
}
