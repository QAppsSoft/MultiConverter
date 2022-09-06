using System;
using System.IO;

namespace MultiConverter.Services.SettingsFixtures;

public sealed class TemporalFile : IDisposable
{
    private readonly string _path;

    private TemporalFile(string filename) => _path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}-{filename}");

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

    public static implicit operator string(TemporalFile temporaryFile) => temporaryFile._path;

    public static TemporalFile Create(string filename) => new(filename);
}
