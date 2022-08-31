namespace MultiConverter.Services.Abstractions.Settings;

public interface IConverter<T>
{
    T Convert(State state);

    State Convert(T state);

    T GetDefaultValue();
}