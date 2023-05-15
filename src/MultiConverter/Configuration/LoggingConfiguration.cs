using Serilog.Events;

namespace MultiConverter.Configuration;

public sealed class LoggingConfiguration
{
    public string LogFileName { get; init; } = null!;

    public long LimitBytes { get; init; }

    public LogEventLevel DefaultLogLevel { get; init; }

    public LogEventLevel MicrosoftLogLevel { get; init; }
}
