using System;
using System.IO;
using Autofac;
using Microsoft.Extensions.Logging;
using MultiConverter.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;

namespace MultiConverter.DependencyInjection.Modules;

public class LoggingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterLoggerFactory(builder);

        builder.RegisterGeneric(typeof(Logger<>))
            .As(typeof(ILogger<>))
            .SingleInstance();
    }

    private static void RegisterLoggerFactory(ContainerBuilder builder) =>
        builder.Register<SerilogLoggerFactory>(x =>
            {
                LoggingConfiguration config = x.Resolve<LoggingConfiguration>();
                string logFilePath = GetLogFileName(config);
                Logger? logger = new LoggerConfiguration()
                    .MinimumLevel.Override("Default", config.DefaultLogLevel)
                    .MinimumLevel.Override("Microsoft", config.MicrosoftLogLevel)
                    .WriteTo.Console()
                    .WriteTo.RollingFile(logFilePath, fileSizeLimitBytes: config.LimitBytes)
                    .CreateLogger();

                SerilogLoggerFactory factory = new(logger);

                return factory;
            }).As<ILoggerFactory>()
            .SingleInstance();

    private static string GetLogFileName(LoggingConfiguration config) =>
        // TODO: central location for paths
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            config.LogFileName);
}
