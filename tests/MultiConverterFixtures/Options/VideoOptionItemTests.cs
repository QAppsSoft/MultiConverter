﻿using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Options;
using NUnit.Framework;

namespace MultiConverterFixtures.Options;

public class VideoOptionItemTests
{
    private static AutoMocker GetAutoMocker(ISchedulerProvider? schedulerProvider = null)
    {
        schedulerProvider ??= new TestSchedulers();

        AutoMocker mocker = new();

        mocker.Use(schedulerProvider);

        return mocker;
    }

    private static void SetupGeneralOptions(AutoMocker mocker, GeneralOptions? generalOptions = null)
    {
        Mock<ISetting<GeneralOptions>> setting = mocker.GetMock<ISetting<GeneralOptions>>();

        if (generalOptions.HasValue)
        {
            setting.SetupGet(x => x.Value).Returns(Observable.Return(generalOptions.Value));
        }
        else
        {
            setting.SetupGet(x => x.Value).Returns(Observable.Return(GeneralOptions.Default()));
        }
    }

    [Test]
    public async Task VideoOptionItem_after_initialization()
    {
        TestSchedulers scheduler = new();
        AutoMocker mocker = GetAutoMocker(scheduler);
        SetupGeneralOptions(mocker);
        using VideoOptionItem fixture = mocker.CreateInstance<VideoOptionItem>();

        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        scheduler.Dispatcher.Start();
        bool hasChanged = await fixture.HasChanged.Take(1);

        fixture.AnalysisTimeout.Should().Be(60);
        fixture.LoadFilesAlreadyInQueue.Should().BeFalse();
        hasChanged.Should().BeFalse();
    }

    [Test]
    public async Task VideoOptionItem_when_changed_AnalysisTimeout_HasChanged_should_be_true()
    {
        const int expectedTimeout = 125;
        TestSchedulers scheduler = new();
        AutoMocker mocker = GetAutoMocker(scheduler);
        SetupGeneralOptions(mocker);
        using VideoOptionItem fixture = mocker.CreateInstance<VideoOptionItem>();

        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        fixture.AnalysisTimeout = expectedTimeout;
        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        scheduler.Dispatcher.Start();
        bool hasChanged = await fixture.HasChanged.Take(1);

        fixture.AnalysisTimeout.Should().Be(expectedTimeout);
        hasChanged.Should().BeTrue();
    }

    [Test]
    public async Task VideoOptionItem_when_changed_LoadFilesAlreadyInQueue_HasChanged_should_be_true()
    {
        const bool expectedValue = true;
        TestSchedulers scheduler = new();
        AutoMocker mocker = GetAutoMocker(scheduler);
        SetupGeneralOptions(mocker);
        using VideoOptionItem fixture = mocker.CreateInstance<VideoOptionItem>();

        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        fixture.LoadFilesAlreadyInQueue = expectedValue;
        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        scheduler.Dispatcher.Start();
        bool hasChanged = await fixture.HasChanged.Take(1);

        fixture.LoadFilesAlreadyInQueue.Should().Be(expectedValue);
        hasChanged.Should().BeTrue();
    }

    [Test]
    public async Task VideoOptionItem_after_restoring_value_HasChanged_should_be_false()
    {
        bool originalValue = GeneralOptions.Default().LoadFilesAlreadyInQueue;
        TestSchedulers scheduler = new();
        AutoMocker mocker = GetAutoMocker(scheduler);
        SetupGeneralOptions(mocker);
        using VideoOptionItem fixture = mocker.CreateInstance<VideoOptionItem>();

        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        fixture.LoadFilesAlreadyInQueue = !originalValue;
        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        fixture.LoadFilesAlreadyInQueue = originalValue;
        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        scheduler.Dispatcher.Start();
        bool hasChanged = await fixture.HasChanged.Take(1);

        fixture.LoadFilesAlreadyInQueue.Should().Be(originalValue);
        hasChanged.Should().BeFalse();
    }

    [Test]
    public async Task VideoOptionItem_when_changed_UpdateOption_should_change_language()
    {
        const int expectedTimeout = 132;
        const bool expectedInQueue = true;
        TestSchedulers scheduler = new();
        AutoMocker mocker = GetAutoMocker(scheduler);
        SetupGeneralOptions(mocker);
        using VideoOptionItem fixture = mocker.CreateInstance<VideoOptionItem>();

        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        fixture.AnalysisTimeout = expectedTimeout;
        fixture.LoadFilesAlreadyInQueue = expectedInQueue;
        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        scheduler.Dispatcher.Start();
        bool hasChanged = await fixture.HasChanged.Take(1);
        GeneralOptions option = fixture.UpdateOption(GeneralOptions.Default());

        fixture.AnalysisTimeout.Should().Be(expectedTimeout);
        fixture.LoadFilesAlreadyInQueue.Should().Be(expectedInQueue);
        hasChanged.Should().BeTrue();
        option.AnalysisTimeout.Should().Be(expectedTimeout);
        option.LoadFilesAlreadyInQueue.Should().Be(expectedInQueue);
    }
}