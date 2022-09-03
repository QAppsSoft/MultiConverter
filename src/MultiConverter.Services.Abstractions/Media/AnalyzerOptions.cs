using System;

namespace MultiConverter.Services.Abstractions.Media;

public readonly record struct AnalyzerOptions(TimeSpan Timeout)
{
    public static AnalyzerOptions Default() => new AnalyzerOptions(TimeSpan.FromMinutes(5));
}
