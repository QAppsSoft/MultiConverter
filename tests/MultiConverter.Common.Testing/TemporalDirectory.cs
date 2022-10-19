using System;
using System.IO;

namespace MultiConverter.Common.Testing;

public class TemporalDirectory : IDisposable
{
    private readonly TemporalDirectoryPath _directoryPath;

    public TemporalDirectory(string directoryName)
    {
        _directoryPath = TemporalDirectoryPath.Create(directoryName);
        Directory.CreateDirectory(_directoryPath);
    }

    public void Dispose() => _directoryPath.Dispose();

    public static implicit operator string(TemporalDirectory temporaryDirectoryPath) => temporaryDirectoryPath._directoryPath;

    public static TemporalDirectory Create(string directoryName) => new(directoryName);
}
