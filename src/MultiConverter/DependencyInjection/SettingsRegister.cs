using System;
using MultiConverter.Services.Abstractions.Settings;
using Splat;

namespace MultiConverter.DependencyInjection;

public sealed class SettingsRegister : ISettingsRegister
{
    private readonly IMutableDependencyResolver _services;
    private readonly ISettingFactory _settingFactory;

    public SettingsRegister(IMutableDependencyResolver services, ISettingFactory settingFactory)
    {
        _services = services;
        _settingFactory = settingFactory ?? throw new ArgumentNullException(nameof(settingFactory));
    }

    public void Register<T>(IConverter<T> converter, string key)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(converter);
        ArgumentNullException.ThrowIfNull(key);

        ISetting<T> setting = _settingFactory.Create(converter, key);

        _services.Register(() => setting);
    }
}
