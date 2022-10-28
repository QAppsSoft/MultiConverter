using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FluentAssertions;
using Moq.AutoMock;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Options;
using MultiConverter.ViewModels.Options.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModelsFixtures.Options;

public class OptionsViewModelTests : OptionsTestBase
{
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
        Subject<bool> hasChanged = new();
        var optionItem = new FakeOptionItem(hasChanged);
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        SetupOptionItems(mocker, new[] { optionItem });
        bool? canExecute = null;
        OptionsViewModel fixture = mocker.CreateInstance<OptionsViewModel>();

        fixture.Activator.Activate();
        _ = fixture.Save?.CanExecute.Subscribe(value => canExecute = value);
        hasChanged.OnNext(true);

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
    public void Save_calls_UpdateOption()
    {
        int? result = null;
        var newTimeout = 123;
        AutoMocker mocker = GetAutoMocker();
        Subject<bool> hasChanged = new();
        var optionItem = new FakeOptionItem(hasChanged, options => options with { AnalysisTimeout = newTimeout });
        SetupGeneralOptions(mocker, GeneralOptions.Default());
        SetupOptionItems(mocker, new[] { optionItem });
        var setting = mocker.GetMock<ISetting<GeneralOptions>>().Object;
        OptionsViewModel fixture = mocker.CreateInstance<OptionsViewModel>();

        fixture.Activator.Activate();
        hasChanged.OnNext(true);
        setting.Value.Subscribe(x => result = x.AnalysisTimeout);
        fixture.Save?.Execute().Subscribe();

        result.Should().Be(newTimeout);
    }

    [Test]
    public void Deactivation_should_set_null_ReactiveCommand()
    {
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        OptionsViewModel fixture = mocker.CreateInstance<OptionsViewModel>();

        fixture.Activator.Activate();
        fixture.Activator.Deactivate();

        fixture.Save.Should().BeNull();
        fixture.Reset.Should().BeNull();
    }
}

public class FakeOptionItem : ReactiveObject, IOptionItem
{
    public FakeOptionItem(IObservable<bool>? hasChanged = null, Func<GeneralOptions, GeneralOptions>? updateOption = null)
    {
        IObservable<bool> changed = hasChanged ?? Observable.Return(false);

        changed.ToPropertyEx(this, vm => vm.HasChanged, scheduler: ImmediateScheduler.Instance);

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

    [ObservableAsProperty] public bool HasChanged { get; }
    public Func<GeneralOptions, GeneralOptions> UpdateOption { get; }
}
