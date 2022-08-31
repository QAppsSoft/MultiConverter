namespace MultiConverter.Services.Abstractions.Settings;

public interface ISettingsStore
{
    State Load(string key);

    void Save(string key, State state);
}