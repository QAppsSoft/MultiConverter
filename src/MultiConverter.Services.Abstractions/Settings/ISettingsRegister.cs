namespace MultiConverter.Services.Abstractions.Settings;

public interface ISettingsRegister
{
    void Register<T>(IConverter<T> converter, string key)
        where T : notnull;
}
