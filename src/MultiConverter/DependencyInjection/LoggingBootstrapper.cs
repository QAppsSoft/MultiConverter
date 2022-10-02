using System;
using System.IO;
using Microsoft.Extensions.Logging;
using MultiConverter.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;
using Splat;

namespace MultiConverter.DependencyInjection;

public static class LoggingBootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.RegisterLazySingleton<ILoggerFactory>(() =>
        {
            LoggingConfiguration config = resolver.GetRequiredService<LoggingConfiguration>();
            string logFilePath = GetLogFileName(config, resolver);
            Logger? logger = new LoggerConfiguration()
                .MinimumLevel.Override("Default", config.DefaultLogLevel)
                .MinimumLevel.Override("Microsoft", config.MicrosoftLogLevel)
                .WriteTo.Console()
                .WriteTo.RollingFile(logFilePath, fileSizeLimitBytes: config.LimitBytes)
                .CreateLogger();

            SerilogLoggerFactory factory = new(logger);

            return factory;
        });

        services.RegisterLazySingleton(() =>
        {
            ILoggerFactory factory = resolver.GetRequiredService<ILoggerFactory>();
            return factory.CreateLogger("Default");
        });
    }

    private static string GetLogFileName(LoggingConfiguration config, IReadonlyDependencyResolver resolver) =>
        // TODO: central location for paths
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            config.LogFileName);
}
