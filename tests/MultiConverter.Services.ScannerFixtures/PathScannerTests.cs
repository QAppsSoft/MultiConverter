using System;
using System.IO;
using System.Reactive.Linq;
using FluentAssertions;
using MultiConverter.Common.Testing;
using MultiConverter.Services.Scanner;
using NUnit.Framework;

namespace MultiConverter.Services.ScannerFixtures;

public class PathScannerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Enumerate_Directory()
    {
        int counter = 0;

        TestSchedulers schedulerProvider = new();
        string systemDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        PathScanner pathScanner = new(systemDirectory, schedulerProvider);

        using IDisposable disposable = pathScanner.Files.Take(10).Count().Subscribe(value => counter = value);

        schedulerProvider.CurrentThread.AdvanceBy(TimeSpan.FromMinutes(1).Ticks);
        schedulerProvider.CurrentThread.Start();

        counter.Should().BeGreaterThan(0);
    }

    [Test]
    public void Enumerate_SingleFile()
    {
        int counter = 0;
        using TemporalFile temporalFile = TemporalFile.Create("EnumerateSingleFile.txt");

        TestSchedulers schedulerProvider = new();
        PathScanner pathScanner = new(temporalFile, schedulerProvider);

        using IDisposable disposable = pathScanner.Files.Count().Subscribe(value => counter = value);

        schedulerProvider.CurrentThread.AdvanceBy(TimeSpan.FromMinutes(1).Ticks);
        schedulerProvider.CurrentThread.Start();

        counter.Should().Be(1);
    }
}
