using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Options;
using MultiConverter.ViewModels.Options.Interfaces;
using NUnit.Framework;

namespace MultiConverterFixtures;

public class OptionsViewModelTests
{
    private static AutoMocker GetAutoMocker(ISchedulerProvider? schedulerProvider = null)
    {
        schedulerProvider ??= new ImmediateSchedulers();

        AutoMocker mocker = new();

        mocker.Use(schedulerProvider);

        return mocker;
    }

    private static void SetupGeneralOptions(AutoMocker mocker, GeneralOptions? generalOptions = null)
    {
        Mock<ISetting<GeneralOptions>> setting = mocker.GetMock<ISetting<GeneralOptions>>();
        ReplaySubject<GeneralOptions> generalOptionsSubject = new(1);

        setting.Setup(x => x.Write(It.IsAny<GeneralOptions>()))
            .Callback<GeneralOptions>(item => generalOptionsSubject.OnNext(item));

        if (generalOptions.HasValue)
        {
            generalOptionsSubject.OnNext(generalOptions.Value);
        }

        setting.SetupGet(x => x.Value).Returns(generalOptionsSubject.AsObservable);

        mocker.Use(setting);
    }

    private static void SetupOptionItems(AutoMocker mocker, IEnumerable<IOptionItem>? optionItems = null)
    {
        if (optionItems == null)
        {
            List<IOptionItem> items = new() { new FakeOptionItem() };
            mocker.Use<IEnumerable<IOptionItem>>(items);
        }
        else
        {
            mocker.Use(optionItems);
        }
    }

    [Test]
    public void Check_viewmodel_status_before_activation()
    {
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        OptionsViewModel fixture = mocker.CreateInstance<OptionsViewModel>();

        fixture.Save.Should().BeNull();
        fixture.Options.Should().BeNull();
        fixture.Reset.Should().BeNull();
    }

    [Test]
    public void Check_viewmodel_status_after_activation()
    {
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        SetupOptionItems(mocker);
        bool? canExecute = null;
        OptionsViewModel fixture = mocker.CreateInstance<OptionsViewModel>();

        fixture.Activator.Activate();
        _ = fixture.Save?.CanExecute.Subscribe(value => canExecute = value);

        canExecute.Should().BeFalse();
        fixture.Options.Should().NotBeEmpty();
    }

    [Test]
    public void Check_viewmodel_can_execute_saveButton_after_activation_and_optionItem_changed()
    {
        Subject<bool> hasChangedSubject = new();
        var optionItem = new FakeOptionItem(hasChangedSubject);
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        SetupOptionItems(mocker, new[] { optionItem });
        bool? canExecute = null;
        OptionsViewModel fixture = mocker.CreateInstance<OptionsViewModel>();

        fixture.Activator.Activate();
        _ = fixture.Save?.CanExecute.Subscribe(value => canExecute = value);
        hasChangedSubject.OnNext(true);

        canExecute.Should().BeTrue();
        fixture.Options.Should().NotBeEmpty();
    }

    [Test]
    public async Task Calling_reset_should_write_GeneralOptions_default()
    {
        AutoMocker mocker = GetAutoMocker();
        var modifiedGeneralOption = GeneralOptions.Default() with { AnalysisTimeout = 120 };
        SetupGeneralOptions(mocker, modifiedGeneralOption);
        SetupOptionItems(mocker);
        OptionsViewModel fixture = mocker.CreateInstance<OptionsViewModel>();
        ISetting<GeneralOptions> setting = mocker.GetMock<ISetting<GeneralOptions>>().Object;

        fixture.Activator.Activate();
        var initialGeneralOption = await setting.Value.Take(1);
        fixture.Reset?.Execute().Subscribe();
        var resetGeneralOption = await setting.Value.Take(1);

        initialGeneralOption.Should().Be(modifiedGeneralOption);
        resetGeneralOption.Should().Be(GeneralOptions.Default());
    }

    [Test]
    public async Task Save_calls_UpdateOption()
    {
        var newTimeout = 123;
        AutoMocker mocker = GetAutoMocker();
        Subject<bool> hasChangedSubject = new();
        var optionItem = new FakeOptionItem(hasChangedSubject, options => options with { AnalysisTimeout = newTimeout });
        SetupGeneralOptions(mocker, GeneralOptions.Default());
        SetupOptionItems(mocker, new[] { optionItem });
        var setting = mocker.GetMock<ISetting<GeneralOptions>>().Object;
        OptionsViewModel fixture = mocker.CreateInstance<OptionsViewModel>();

        fixture.Activator.Activate();
        fixture.Save?.Execute().Subscribe();
        var result = await setting.Value.Take(1);

        result.AnalysisTimeout.Should().Be(newTimeout);
    }
}

public class FakeOptionItem : IOptionItem
{
    public FakeOptionItem(IObservable<bool>? hasChanged = null, Func<GeneralOptions, GeneralOptions>? updateOption = null)
    {
        HasChanged = hasChanged ?? Observable.Return(false);
        UpdateOption = BuildUpdateOptionFunc(updateOption);
    }

    private Func<GeneralOptions, GeneralOptions> BuildUpdateOptionFunc(
        Func<GeneralOptions, GeneralOptions>? updateOption)
    {
        if (updateOption == null)
        {
            return options =>
            {
                UpdateCalled = true;
                return options;
            };
        }

        return option =>
        {
            UpdateCalled = true;
            return updateOption(option);
        };
    }

    public bool UpdateCalled { get; private set; }

    public void ResetUpdateStatus() => UpdateCalled = false;

    public IObservable<bool> HasChanged { get; }
    public Func<GeneralOptions, GeneralOptions> UpdateOption { get; }
}
