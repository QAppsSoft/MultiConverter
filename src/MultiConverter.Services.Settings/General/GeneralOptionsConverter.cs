using System.Text.Json;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;

namespace MultiConverter.Services.Settings.General;

public class GeneralOptionsConverter : IConverter<GeneralOptions>
{
    public GeneralOptions Convert(State state)
    {
        GeneralOptions defaults = GetDefaultValue();

        if (state == State.Empty)
        {
            return defaults;
        }

        GeneralOptions generalOptions = JsonSerializer.Deserialize<GeneralOptions>(state.Value);

        // if (generalOptions == default)
        // {
        //     return defaults;
        // }

        return state.Version switch
        {
            1 => generalOptions,
            _ => defaults
        };
    }

    public State Convert(GeneralOptions generalOptions)
    {
        //generalOptions == default
        //? State.Empty:
        var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
        return new State(1, JsonSerializer.Serialize(generalOptions, serializerOptions));
    }

    public GeneralOptions GetDefaultValue() => GeneralOptions.Default();
}
