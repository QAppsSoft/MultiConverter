using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services;
using MultiConverter.ViewModels.Options;
using NUnit.Framework;

namespace MultiConverterFixtures.Options;

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
        SetupDialogService(mocker,expectedPath);
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
        SetupDialogService(mocker, selectedPath);
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
        SetupDialogService(mocker, expectedPath);
        TemporalPathOptionItem fixture = mocker.CreateInstance<TemporalPathOptionItem>();

        fixture.ChangeTemporalPath.Execute();
        GeneralOptions expectedResult = fixture.UpdateOption(GeneralOptions.Default());

        fixture.TemporalPath.Should().Be(expectedPath);
        expectedResult.Should().Be(GeneralOptions.Default() with { TemporalFolder = expectedPath });
    }
}
