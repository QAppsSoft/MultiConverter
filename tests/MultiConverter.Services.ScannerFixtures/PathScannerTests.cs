using System;
using System.IO;
using System.Reactive.Linq;
using AwesomeAssertions;
using MultiConverter.Common.Testing;
using MultiConverter.Services.Scanner;
using NUnit.Framework;

namespace MultiConverter.Services.ScannerFixtures;

public class PathScannerTests
{
    [Test]
    public void Enumerate_Directory()
    {
        int counter = 0;

        ImmediateSchedulers schedulerProvider = new();
        string systemDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        PathScanner pathScanner = new(systemDirectory, schedulerProvider);

        using IDisposable disposable = pathScanner.Files.Take(10).Count().Subscribe(value => counter = value);

        counter.Should().BeGreaterThan(0);
    }

    [Test]
    public void Enumerate_SingleFile()
    {
        int counter = 0;
        using TemporalFile temporalFile = TemporalFile.Create("EnumerateSingleFile.txt");

        ImmediateSchedulers schedulerProvider = new();
        PathScanner pathScanner = new(temporalFile, schedulerProvider);

        using IDisposable disposable = pathScanner.Files.Count().Subscribe(value => counter = value);

        counter.Should().Be(1);
    }
}
