using Microsoft.Extensions.Logging;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.Services.Settings;
using MultiConverter.Services.Settings.General;
using Splat;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace MultiConverter.DependencyInjection;

public static class SettingsBootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        RegisterCommonInterfaces(services, resolver);
        RegisterConverters(services, resolver);
        RegisterSettings(services, resolver);
    }

    private static void RegisterConverters(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver) =>
        services.Register<IConverter<GeneralOptions>>(() => new GeneralOptionsConverter());

    private static void RegisterSettings(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        ISettingsRegister settingRegister = resolver.GetRequiredService<ISettingsRegister>();

        IConverter<GeneralOptions> generalOptionsConverter = resolver.GetRequiredService<IConverter<GeneralOptions>>();

        settingRegister.Register(generalOptionsConverter, "GeneralOptions");
    }

    private static void RegisterCommonInterfaces(IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        services.Register<ISettingFactory>(() => new SettingFactory(
            resolver.GetRequiredService<ILoggerFactory>(),
            resolver.GetRequiredService<ISettingsStore>()
        ));

        services.Register<ISettingsStore>(() => new FileSettingsStore(
            resolver.GetRequiredService<ILogger>()
        ));

        services.Register<ISettingsRegister>(() => new SettingsRegister(
            services,
            resolver.GetRequiredService<ISettingFactory>()
        ));
    }
}
