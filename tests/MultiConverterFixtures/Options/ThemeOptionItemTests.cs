using System;
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

public class ThemeOptionItemTests
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
    public async Task ThemeOptionItem_after_initialization()
    {
        TestSchedulers scheduler = new();
        AutoMocker mocker = GetAutoMocker(scheduler);
        SetupGeneralOptions(mocker);
        using ThemeOptionItem fixture = mocker.CreateInstance<ThemeOptionItem>();

        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        scheduler.Dispatcher.Start();
        bool hasChanged = await fixture.HasChanged.Take(1);

        fixture.Themes.Count().Should().Be(2);
        fixture.SelectedTheme.Should().Be(Theme.Dark);
        hasChanged.Should().BeFalse();
    }

    [Test]
    public async Task ThemeOptionItem_when_changed_HasChanged_should_be_true()
    {
        TestSchedulers scheduler = new();
        AutoMocker mocker = GetAutoMocker(scheduler);
        SetupGeneralOptions(mocker);
        using ThemeOptionItem fixture = mocker.CreateInstance<ThemeOptionItem>();

        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        fixture.SelectedTheme = Theme.Light;
        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        scheduler.Dispatcher.Start();
        bool hasChanged = await fixture.HasChanged.Take(1);

        fixture.SelectedTheme.Should().Be(Theme.Light);
        hasChanged.Should().BeTrue();
    }

    [Test]
    public async Task ThemeOptionItem_when_changed_UpdateOption_should_change_theme()
    {
        TestSchedulers scheduler = new();
        AutoMocker mocker = GetAutoMocker(scheduler);
        SetupGeneralOptions(mocker);
        using ThemeOptionItem fixture = mocker.CreateInstance<ThemeOptionItem>();

        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        fixture.SelectedTheme = Theme.Light;
        scheduler.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        scheduler.Dispatcher.Start();
        bool hasChanged = await fixture.HasChanged.Take(1);
        GeneralOptions option = fixture.UpdateOption(GeneralOptions.Default());

        fixture.SelectedTheme.Should().Be(Theme.Light);
        hasChanged.Should().BeTrue();
        option.Theme.Should().Be(Theme.Light);
    }
}
