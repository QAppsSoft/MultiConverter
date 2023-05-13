using FluentAssertions;
using Moq.AutoMock;
using MultiConverter.Models.Settings.General;
using MultiConverter.ViewModels.Settings;

namespace MultiConverter.ViewModelsFixtures.Settings;

public class VideoOptionItemTests : OptionsTestBase
{
    [Test]
    public void VideoOptionItem_after_initialization()
    {
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using VideoSettingItem fixture = mocker.CreateInstance<VideoSettingItem>();

        fixture.AnalysisTimeout.Should().Be(60);
        fixture.LoadFilesAlreadyInQueue.Should().BeFalse();
        fixture.HasChanged.Should().BeFalse();
    }

    [Test]
    public void VideoOptionItem_when_changed_AnalysisTimeout_HasChanged_should_be_true()
    {
        const int expectedTimeout = 125;
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using VideoSettingItem fixture = mocker.CreateInstance<VideoSettingItem>();

        fixture.AnalysisTimeout = expectedTimeout;

        fixture.AnalysisTimeout.Should().Be(expectedTimeout);
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void VideoOptionItem_when_changed_LoadFilesAlreadyInQueue_HasChanged_should_be_true()
    {
        const bool expectedValue = true;
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using VideoSettingItem fixture = mocker.CreateInstance<VideoSettingItem>();

        fixture.LoadFilesAlreadyInQueue = expectedValue;

        fixture.LoadFilesAlreadyInQueue.Should().Be(expectedValue);
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void VideoOptionItem_after_restoring_value_HasChanged_should_be_false()
    {
        bool originalValue = GeneralOptions.Default().LoadFilesAlreadyInQueue;
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using VideoSettingItem fixture = mocker.CreateInstance<VideoSettingItem>();

        fixture.LoadFilesAlreadyInQueue = !originalValue;
        fixture.LoadFilesAlreadyInQueue = originalValue;

        fixture.LoadFilesAlreadyInQueue.Should().Be(originalValue);
        fixture.HasChanged.Should().BeFalse();
    }

    [Test]
    public void VideoOptionItem_when_changed_UpdateOption_should_change_language()
    {
        const int expectedTimeout = 132;
        const bool expectedInQueue = true;
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using VideoSettingItem fixture = mocker.CreateInstance<VideoSettingItem>();

        fixture.AnalysisTimeout = expectedTimeout;
        fixture.LoadFilesAlreadyInQueue = expectedInQueue;
        GeneralOptions option = fixture.UpdateOption(GeneralOptions.Default());

        fixture.AnalysisTimeout.Should().Be(expectedTimeout);
        fixture.LoadFilesAlreadyInQueue.Should().Be(expectedInQueue);
        fixture.HasChanged.Should().BeTrue();
        option.AnalysisTimeout.Should().Be(expectedTimeout);
        option.LoadFilesAlreadyInQueue.Should().Be(expectedInQueue);
    }
}
