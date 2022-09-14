using System;

namespace MultiConverter.Services.Abstractions.Scanner;

public interface IPathScanner
{
    IObservable<string> Files { get; }
}
