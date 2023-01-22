using Autofac;
using Microsoft.Extensions.Configuration;
using MultiConverter.Configuration;

namespace MultiConverter.DependencyInjection.Modules;

public class ConfigurationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterConfiguration(builder);
        RegisterLoggingConfiguration(builder);
        RegisterLanguagesConfiguration(builder);
    }

    private static void RegisterLanguagesConfiguration(ContainerBuilder builder) =>
        builder.Register(x =>
        {
            var configuration = x.Resolve<IConfiguration>();
            LanguagesConfiguration languagesConfiguration = new();
            configuration.GetSection("Languages").Bind(languagesConfiguration);
            return languagesConfiguration;
        });

    private static void RegisterLoggingConfiguration(ContainerBuilder builder) =>
        builder.Register(x =>
        {
            var configuration = x.Resolve<IConfiguration>();
            LoggingConfiguration loggingConfiguration = new();
            configuration.GetSection("Logging").Bind(loggingConfiguration);
            return loggingConfiguration;
        });

    private static void RegisterConfiguration(ContainerBuilder builder) =>
        builder.Register(x => new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build())
            .As<IConfiguration>()
            .SingleInstance();
}
