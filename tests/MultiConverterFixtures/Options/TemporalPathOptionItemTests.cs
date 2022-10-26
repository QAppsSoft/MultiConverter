using System;
using System.ComponentModel;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using Moq;
using Moq.AutoMock;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Options;
using NUnit.Framework;

namespace MultiConverterFixtures.Options;

public class TemporalPathOptionItemTests
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
    public void TemporalPathOptionItem_after_initialization()
    {
        string expectedValue = Path.GetTempPath();
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using TemporalPathOptionItem fixture = mocker.CreateInstance<TemporalPathOptionItem>();

        fixture.TemporalPath.Should().Be(expectedValue);
        fixture.HasChanged.Should().BeFalse();
    }

    [Test]
    public void TemporalPathOptionItem_when_changed_HasChanged_should_be_true()
    {
        using TemporalDirectory expectedPath = TemporalDirectory.Create("has_changed");
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using TemporalPathOptionItem fixture = mocker.CreateInstance<TemporalPathOptionItem>();

        fixture.TemporalPath = expectedPath;

        fixture.TemporalPath.Should().Be(expectedPath);
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void Check_temporalPath_command_execution()
    {
        using TemporalDirectory expectedPath = TemporalDirectory.Create("command_execution");
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        Mock<ISetting<GeneralOptions>> setting = mocker.GetMock<ISetting<GeneralOptions>>();
        setting.SetupGet(x => x.Value).Returns(Observable.Return(GeneralOptions.Default()));

        Mock<IDialogService> dialogService = mocker.GetMock<IDialogService>();
        dialogService.Setup(x => x.DialogManager.ShowFrameworkDialogAsync(
                It.IsAny<INotifyPropertyChanged?>(),
                It.IsAny<OpenFolderDialogSettings>(),
                It.IsAny<AppDialogSettingsBase>(),
                It.IsAny<Func<object?, string>?>()))
            .ReturnsAsync((string)expectedPath);

        TemporalPathOptionItem fixture = mocker.CreateInstance<TemporalPathOptionItem>();


        fixture.ChangeTemporalPath.Execute();


        fixture.TemporalPath.Should().Be(expectedPath);
    }

    [Test]
    public void Check_if_new_temporalPath_isNull_TemporalPath_should_not_change()
    {
        string? selectedPath = null;
        string expectedPath = GeneralOptions.Default().TemporalFolder;
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        Mock<ISetting<GeneralOptions>> setting = mocker.GetMock<ISetting<GeneralOptions>>();
        setting.SetupGet(x => x.Value).Returns(Observable.Return(GeneralOptions.Default()));

        Mock<IDialogService> dialogService = mocker.GetMock<IDialogService>();
        dialogService.Setup(x => x.DialogManager.ShowFrameworkDialogAsync(
                It.IsAny<INotifyPropertyChanged?>(),
                It.IsAny<OpenFolderDialogSettings>(),
                It.IsAny<AppDialogSettingsBase>(),
                It.IsAny<Func<object?, string>?>()))
            .ReturnsAsync(selectedPath);

        TemporalPathOptionItem fixture = mocker.CreateInstance<TemporalPathOptionItem>();


        fixture.ChangeTemporalPath.Execute();


        fixture.TemporalPath.Should().Be(expectedPath);
    }

    [Test]
    public async Task When_changed_UpdateOption_should_change_temporalPath()
    {
        using TemporalDirectory expectedPath = TemporalDirectory.Create("command_execution");
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        Mock<ISetting<GeneralOptions>> setting = mocker.GetMock<ISetting<GeneralOptions>>();
        setting.SetupGet(x => x.Value).Returns(Observable.Return(GeneralOptions.Default()));

        Mock<IDialogService> dialogService = mocker.GetMock<IDialogService>();
        dialogService.Setup(x => x.DialogManager.ShowFrameworkDialogAsync(
                It.IsAny<INotifyPropertyChanged?>(),
                It.IsAny<OpenFolderDialogSettings>(),
                It.IsAny<AppDialogSettingsBase>(),
                It.IsAny<Func<object?, string>?>()))
            .ReturnsAsync((string)expectedPath);

        TemporalPathOptionItem fixture = mocker.CreateInstance<TemporalPathOptionItem>();


        fixture.ChangeTemporalPath.Execute();
        GeneralOptions expectedResult = fixture.UpdateOption(GeneralOptions.Default());


        fixture.TemporalPath.Should().Be(expectedPath);
        expectedResult.Should().Be(GeneralOptions.Default() with { TemporalFolder = expectedPath });
    }
}
