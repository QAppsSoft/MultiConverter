using Autofac;
using MultiConverter.DependencyInjection.Modules;

namespace MultiConverter.DependencyInjection;

public static class Bootstrapper
{
    public static void Register(ContainerBuilder builder)
    {
        builder.RegisterModule<ConfigurationModule>();
        builder.RegisterModule<LoggingModule>();
        builder.RegisterModule<SettingsModule>();
        builder.RegisterModule<ViewModelsModule>();
        builder.RegisterModule<ServicesModule>();
        builder.RegisterModule<PresetsModule>();
    }
}
