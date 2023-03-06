using FluentAssertions;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Presets.Options;
using MultiConverter.ViewModels.Presets.Options;

namespace MultiConverter.ViewModelsFixtures.Presets;

[TestFixture]
public class VideoFrameRateOptionViewModelTests
{
    private const double DefaultFrameRate = 29.97;


    [Test]
    public void New_VideoFrameRateOptionViewModel_After_Initialization()
    {
        VideoFrameRateOptionViewModel fixture = InitializeFixture();

        fixture.FrameRate.Should().Be(DefaultFrameRate);
        fixture.HasChanged.Should().BeFalse();
        fixture.DefaultOptions.Length.Should().Be(12);
    }

    [Test]
    public void After_value_changed_HasChanged_should_be_true()
    {
        const double newFrameRate = 120;
        VideoFrameRateOptionViewModel fixture = InitializeFixture();

        fixture.FrameRate = newFrameRate;

        fixture.HasChanged.Should().BeTrue();
        fixture.FrameRate.Should().Be(newFrameRate);
    }

    [Test]
    [TestCase(0, 8)]
    [TestCase(1, 12)]
    [TestCase(2, 15)]
    [TestCase(3, 23.976)]
    [TestCase(4, 24)]
    [TestCase(5, 25)]
    [TestCase(6, 29.97)]
    [TestCase(7, 30)]
    [TestCase(8, 50)]
    [TestCase(9, 59.94)]
    [TestCase(10, 60)]
    [TestCase(11, 120)]
    public void Executing_ValuesUpdater_should_update_value(int index, double frameRate)
    {
        VideoFrameRateOptionViewModel fixture = InitializeFixture(0);
        ValuesUpdater updater = fixture.DefaultOptions[index];

        updater.Update.Invoke();

        fixture.FrameRate.Should().Be(frameRate);
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void After_set_initial_value_HasChanged_should_be_false()
    {
        VideoFrameRateOptionViewModel fixture = InitializeFixture();

        fixture.FrameRate = 120;
        fixture.FrameRate = DefaultFrameRate;

        fixture.HasChanged.Should().BeFalse();
        fixture.FrameRate.Should().Be(DefaultFrameRate);
    }

    private static VideoFrameRateOptionViewModel InitializeFixture(double? initialFrameRate = null)
    {
        ISchedulerProvider scheduler = new ImmediateSchedulers();
        VideoFrameRateOption option = new(initialFrameRate ?? DefaultFrameRate);
        return new VideoFrameRateOptionViewModel(option, scheduler);
    }
}
