using Microsoft.Extensions.Configuration;
using MultiConverter.Configuration;
using Splat;

namespace MultiConverter.DependencyInjection;

public static class ConfigurationBootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        IConfiguration configuration = BuildConfiguration();

        RegisterLoggingConfiguration(services, configuration);
        RegisterLanguagesConfiguration(services, configuration);
    }

    private static IConfiguration BuildConfiguration() =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

    private static void RegisterLoggingConfiguration(IMutableDependencyResolver services, IConfiguration configuration)
    {
        LoggingConfiguration config = new();
        configuration.GetSection("Logging").Bind(config);
        services.RegisterConstant(config);
    }

    private static void RegisterLanguagesConfiguration(IMutableDependencyResolver services,
        IConfiguration configuration)
    {
        var config = new LanguagesConfiguration();
        configuration.GetSection("Languages").Bind(config);
        services.RegisterConstant(config);
    }
}
