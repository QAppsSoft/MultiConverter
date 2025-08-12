using AwesomeAssertions;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Presets.Options;
using MultiConverter.ViewModels.Presets.Options;

namespace MultiConverter.ViewModelsFixtures.Presets.Options;

[TestFixture]
public class VideoBitrateOptionViewModelTests
{
    private const int DefaultBitrate = 650;

    [Test]
    public void New_VideoBitrateOptionViewModel_After_Initialization()
    {
        VideoBitrateOptionViewModel fixture = InitializeFixture();

        fixture.Bitrate.Should().Be(DefaultBitrate);
        fixture.HasChanged.Should().BeFalse();
        fixture.DefaultOptions.Length.Should().Be(0);
    }

    [Test]
    public void After_value_changed_HasChanged_should_be_true()
    {
        const int newFrameRate = 760;
        VideoBitrateOptionViewModel fixture = InitializeFixture();

        fixture.Bitrate = newFrameRate;

        fixture.HasChanged.Should().BeTrue();
        fixture.Bitrate.Should().Be(newFrameRate);
    }

    [Test]
    public void After_set_initial_value_HasChanged_should_be_false()
    {
        VideoBitrateOptionViewModel fixture = InitializeFixture();

        fixture.Bitrate = 120;
        fixture.Bitrate = DefaultBitrate;

        fixture.HasChanged.Should().BeFalse();
        fixture.Bitrate.Should().Be(DefaultBitrate);
    }

    private static VideoBitrateOptionViewModel InitializeFixture(int? initialBitrate = null)
    {
        ISchedulerProvider scheduler = new ImmediateSchedulers();
        VideoBitrateOption option = new(initialBitrate ?? DefaultBitrate);
        return new VideoBitrateOptionViewModel(option, scheduler);
    }
}
