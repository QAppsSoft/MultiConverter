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
    }

    private static IConfiguration BuildConfiguration() =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

    private static void RegisterLoggingConfiguration(IMutableDependencyResolver services, IConfiguration configuration)
    {
        LoggingConfiguration config = new LoggingConfiguration();
        configuration.GetSection("Logging").Bind(config);
        services.RegisterConstant(config);
    }
}
