using System.Text.Json;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;

namespace MultiConverter.Services.Settings.General;

public class GeneralOptionsConverter : IConverter<GeneralOptions>
{
    private static readonly JsonSerializerOptions s_serializerOptions;

    static GeneralOptionsConverter() =>
        s_serializerOptions = new JsonSerializerOptions { WriteIndented = true };

    public GeneralOptions Convert(State state)
    {
        GeneralOptions defaults = GetDefaultValue();

        if (state == State.Empty)
        {
            return defaults;
        }

        GeneralOptions generalOptions = JsonSerializer.Deserialize<GeneralOptions>(state.Value);

        if (generalOptions == default)
        {
            return defaults;
        }

        return state.Version switch
        {
            1 => generalOptions,
            _ => defaults
        };
    }

    public State Convert(GeneralOptions generalOptions) =>
        generalOptions == default
            ? State.Empty
            : new State(1, JsonSerializer.Serialize(generalOptions, s_serializerOptions));

    public GeneralOptions GetDefaultValue() => GeneralOptions.Default();
}
