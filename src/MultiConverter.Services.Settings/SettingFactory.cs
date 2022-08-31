using System;
using Microsoft.Extensions.Logging;
using MultiConverter.Services.Abstractions.Settings;

namespace MultiConverter.Services.Settings;

public sealed class SettingFactory : ISettingFactory
{
    private readonly ILoggerFactory _logFactory;
    private readonly ISettingsStore _settingsStore;

    public SettingFactory(ILoggerFactory logFactory, ISettingsStore settingsStore)
    {
        _logFactory = logFactory ?? throw new ArgumentNullException(nameof(logFactory));
        _settingsStore = settingsStore ?? throw new ArgumentNullException(nameof(settingsStore));
    }

    public ISetting<T> Create<T>(IConverter<T> converter, string key)
        where T : notnull
    {
        //TODO: Cache stored setting and retrieve if required elsewhere
        return new Setting<T>(_logFactory.CreateLogger<T>(), _settingsStore, converter, key);
    }
}