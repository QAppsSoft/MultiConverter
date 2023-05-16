using Autofac;
using Microsoft.Extensions.Configuration;
using MultiConverter.Configuration;
using MultiConverter.Models.Configurations;

namespace MultiConverter.DependencyInjection.Modules;

public class ConfigurationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterConfiguration(builder);
        RegisterLoggingConfiguration(builder);
        RegisterLanguagesConfiguration(builder);
        RegisterFavoriteFormatsConfiguration(builder);
        RegisterFavoriteCodecsConfiguration(builder);
    }

    private static void RegisterFavoriteCodecsConfiguration(ContainerBuilder builder) =>
        builder.Register(context =>
            {
                var configuration = context.Resolve<IConfiguration>();
                FavoriteCodecsConfiguration codecsConfiguration = new();
                configuration.GetSection("FavoriteCodecs").Bind(codecsConfiguration);
                return codecsConfiguration;
            }
        );


    private static void RegisterFavoriteFormatsConfiguration(ContainerBuilder builder) =>
        builder.Register(context =>
            {
                var configuration = context.Resolve<IConfiguration>();
                FavoriteFormatsConfiguration formatsConfiguration = new();
                configuration.GetSection("FavoriteFormats").Bind(formatsConfiguration);
                return formatsConfiguration;
            }
        );

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
