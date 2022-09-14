using System;
using System.IO;

namespace MultiConverter.Common.Testing;

public sealed class TemporalFile : IDisposable
{
    private readonly TemporalFilePath _filePath;

    private TemporalFile(string filename)
    {
        _filePath = TemporalFilePath.Create(filename);
        File.Create(_filePath).Close();
    }

    public void Dispose()
    {
        _filePath.Dispose();
    }

    public static implicit operator string(TemporalFile temporaryFilePath) => temporaryFilePath._filePath;

    public static TemporalFile Create(string filename) => new(filename);
}
