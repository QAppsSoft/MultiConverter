using FluentAssertions;
using Moq;
using Moq.AutoMock;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services;
using MultiConverter.ViewModels.Settings;

namespace MultiConverter.ViewModelsFixtures.Settings;

public class TemporalPathOptionItemTests : OptionsTestBase
{
    private static void SetupDialogService(AutoMocker mocker, string? selectedPath)
    {
        Mock<IDialogService> dialogService = mocker.GetMock<IDialogService>();
        dialogService.Setup(x => x.ShowFolderSelectorAsync(It.IsAny<FolderDialogSettings?>()))
            .ReturnsAsync(selectedPath);
    }

    [Test]
    public void TemporalPathOptionItem_after_initialization()
    {
        string expectedPath = Path.GetTempPath();
        bool expectedCheck = GeneralOptions.Default().CheckTemporalFolder;
        int expectedEvery = GeneralOptions.Default().CheckTemporalFolderEvery;
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using TemporalPathSettingItem fixture = mocker.CreateInstance<TemporalPathSettingItem>();

        fixture.TemporalPath.Should().Be(expectedPath);
        fixture.CheckTemporalPath.Should().Be(expectedCheck);
        fixture.CheckTemporalPathEvery.Should().Be(expectedEvery);
        fixture.HasChanged.Should().BeFalse();
    }

    [Test]
    public void TemporalPath_when_changed_HasChanged_should_be_true()
    {
        using TemporalDirectory expectedPath = TemporalDirectory.Create("has_changed");
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using TemporalPathSettingItem fixture = mocker.CreateInstance<TemporalPathSettingItem>();

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
        SetupDialogService(mocker,expectedPath);
        TemporalPathSettingItem fixture = mocker.CreateInstance<TemporalPathSettingItem>();

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
        SetupDialogService(mocker, selectedPath);
        TemporalPathSettingItem fixture = mocker.CreateInstance<TemporalPathSettingItem>();

        fixture.ChangeTemporalPath.Execute();

        fixture.TemporalPath.Should().Be(expectedPath);
    }

    [Test]
    public async Task When_changed_UpdateOption_should_change_temporalPath()
    {
        using TemporalDirectory expectedPath = TemporalDirectory.Create("command_execution");
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        SetupDialogService(mocker, expectedPath);
        TemporalPathSettingItem fixture = mocker.CreateInstance<TemporalPathSettingItem>();

        fixture.ChangeTemporalPath.Execute();
        GeneralOptions expectedResult = fixture.UpdateOption(GeneralOptions.Default());

        fixture.TemporalPath.Should().Be(expectedPath);
        expectedResult.Should().Be(GeneralOptions.Default() with { TemporalFolder = expectedPath });
    }

    [Test]
    public void CheckTemporalFolder_when_changed_HasChanged_should_be_true()
    {
        bool expected = !GeneralOptions.Default().CheckTemporalFolder;
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using TemporalPathSettingItem fixture = mocker.CreateInstance<TemporalPathSettingItem>();

        fixture.CheckTemporalPath = expected;

        fixture.CheckTemporalPath.Should().Be(expected);
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void CheckTemporalPathEvery_when_changed_HasChanged_should_be_true()
    {
        int expected = 76;
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using TemporalPathSettingItem fixture = mocker.CreateInstance<TemporalPathSettingItem>();

        fixture.CheckTemporalPathEvery = expected;

        fixture.CheckTemporalPathEvery.Should().Be(expected);
        fixture.HasChanged.Should().BeTrue();
    }
}
