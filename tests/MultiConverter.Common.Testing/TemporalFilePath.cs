using System;
using System.IO;

namespace MultiConverter.Common.Testing;

public sealed class TemporalFilePath : IDisposable
{
    private readonly string _path;

    private TemporalFilePath(string filename) =>
        _path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}-{filename}");

    public void Dispose()
    {
        if (!File.Exists(_path))
        {
            return;
        }

        try
        {
            File.Delete(_path);
        }
        catch
        {
            // ignored
        }
    }

    public static implicit operator string(TemporalFilePath temporaryFilePath) => temporaryFilePath._path;

    public static TemporalFilePath Create(string filename) => new(filename);
}
