using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;
using MultiConverter.Services.Abstractions.Settings;

namespace MultiConverter.Services.Settings;

public class Setting<T> : IEquatable<Setting<T>>, ISetting<T>, IDisposable
    where T : notnull
{
    private readonly ISubject<T> _changed = new ReplaySubject<T>(1);
    private readonly IConverter<T> _converter;
    private readonly string _key;
    private readonly ILogger _logger;
    private readonly ISettingsStore _settingsStore;
    private string _rawValue;
    private T _value;

    public Setting(ILogger logger, ISettingsStore settingsStore, IConverter<T> converter, string key)
    {
        _logger = logger;
        _settingsStore = settingsStore;
        _converter = converter;
        _key = key;

        try
        {
            //make this awaitable
            var state = _settingsStore.Load(_key);
            _rawValue = state.Value;
            _value = converter.Convert(state);
        }
        catch (Exception ex)
        {
            _value = converter.GetDefaultValue();
            _rawValue = converter.Convert(_value).Value;
            _logger.LogError(ex, "Problem reading {Key}", _key);
        }
        _changed.OnNext(_value);
    }

    public IObservable<T> Value => _changed.AsObservable();

    public void Dispose()
    {
        _changed.OnCompleted();
    }

    public override string ToString()
    {
        return _key;
    }

    public void Write(T value)
    {
        var converted = _converter.Convert(value);

        if (_rawValue == converted.Value) return;

        _rawValue = converted.Value;
        _value = value;

        try
        {
            //make this awaitable
            _settingsStore.Save(_key, converted);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Problem writing {Key}", value);
        }
        _changed.OnNext(value);
    }

    #region Equality

    public static bool operator !=(Setting<T> left, Setting<T> right)
    {
        return !Equals(left, right);
    }

    public static bool operator ==(Setting<T> left, Setting<T> right)
    {
        return Equals(left, right);
    }

    public bool Equals(Setting<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(_key, other._key);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Setting<T>)obj);
    }

    public override int GetHashCode()
    {
        return _key.GetHashCode();
    }

    #endregion Equality
}
