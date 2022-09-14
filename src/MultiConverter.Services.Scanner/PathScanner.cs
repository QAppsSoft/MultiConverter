using System;
using System.IO;
using System.Reactive.Linq;
using MultiConverter.Common;
using MultiConverter.Services.Abstractions.Scanner;

namespace MultiConverter.Services.Scanner;

public class PathScanner : IPathScanner
{
    private const string SearchPattern = "*";

    public PathScanner(string path, ISchedulerProvider schedulerProvider)
    {
        if (File.Exists(path))
        {
            Files = Observable.Return(path);
        }
        else
        {
            EnumerationOptions enumerationOptions = new() { RecurseSubdirectories = true, BufferSize = 16 };

            Files = Directory.EnumerateFiles(path, SearchPattern, enumerationOptions)
                .ToObservable(schedulerProvider.CurrentThread)
                .Where(File.Exists);
        }
    }

    public IObservable<string> Files { get; }
}
