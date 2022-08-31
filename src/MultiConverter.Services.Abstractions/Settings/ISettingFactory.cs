namespace MultiConverter.Services.Abstractions.Settings;

public interface ISettingFactory
{
    ISetting<T> Create<T>(IConverter<T> converter, string key)
        where T : notnull;
}