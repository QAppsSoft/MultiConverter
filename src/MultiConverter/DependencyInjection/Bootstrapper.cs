using Splat;

namespace MultiConverter.DependencyInjection;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        ConfigurationBootstrapper.Register(services, resolver);
        LoggingBootstrapper.Register(services, resolver);
        SettingsBootstrapper.Register(services, resolver);
        ViewModelsBootstrapper.Register(services, resolver);
        ServicesBootstrapper.Register(services, resolver);
    }
}
