using FluentAssertions;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Presets.Options;
using MultiConverter.ViewModels.Presets.Options;

namespace MultiConverter.ViewModelsFixtures.Presets;

[TestFixture]
public class AudioBitrateOptionViewModelTests
{
    [Test]
    public void New_VideoFrameRateOptionViewModel_After_Initialization()
    {
        AudioBitrateOptionViewModel fixture = InitializeFixture();

        fixture.Bitrate.Should().Be(DefaultBitrate);
        fixture.HasChanged.Should().BeFalse();
        fixture.DefaultOptions.Length.Should().Be(4);
    }

    [Test]
    public void After_value_changed_HasChanged_should_be_true()
    {
        const int newBitrate = 96;
        AudioBitrateOptionViewModel fixture = InitializeFixture();

        fixture.Bitrate = newBitrate;

        fixture.HasChanged.Should().BeTrue();
        fixture.Bitrate.Should().Be(newBitrate);
    }

    [Test]
    [TestCase(0, 128)]
    [TestCase(1, 196)]
    [TestCase(2, 256)]
    [TestCase(3, 320)]
    public void Executing_ValuesUpdater_should_update_value(int index, int bitrate)
    {
        AudioBitrateOptionViewModel fixture = InitializeFixture(0);
        ValuesUpdater updater = fixture.DefaultOptions[index];

        updater.Update.Invoke();

        fixture.Bitrate.Should().Be(bitrate);
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void After_set_initial_value_HasChanged_should_be_false()
    {
        AudioBitrateOptionViewModel fixture = InitializeFixture();

        fixture.Bitrate = 32;
        fixture.Bitrate = DefaultBitrate;

        fixture.HasChanged.Should().BeFalse();
        fixture.Bitrate.Should().Be(DefaultBitrate);
    }


    private static AudioBitrateOptionViewModel InitializeFixture(int? initialBitrate = null)
    {
        ISchedulerProvider scheduler = new ImmediateSchedulers();
        AudioBitrateOption option = new(initialBitrate ?? DefaultBitrate);
        return new AudioBitrateOptionViewModel(option, scheduler);
    }

    private const int DefaultBitrate = 128;
}

