using System;
using System.IO;

namespace MultiConverter.Common.Testing;

public class TemporalDirectoryPath : IDisposable
{
    private readonly string _path;

    public TemporalDirectoryPath(string directoryName) =>
        _path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}-{directoryName}");


    public void Dispose()
    {
        if (!Directory.Exists(_path))
        {
            return;
        }

        try
        {
            Directory.Delete(_path);
        }
        catch
        {
            // Ignored
        }
    }

    public static implicit operator string(TemporalDirectoryPath temporaryDirectoryPath) =>
        temporaryDirectoryPath._path;

    public static TemporalDirectoryPath Create(string directoryName) => new(directoryName);
}
