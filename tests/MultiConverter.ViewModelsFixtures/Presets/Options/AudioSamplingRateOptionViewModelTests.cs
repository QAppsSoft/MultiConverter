using FluentAssertions;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Presets.Options;
using MultiConverter.ViewModels.Presets.Options;

namespace MultiConverter.ViewModelsFixtures.Presets.Options;

[TestFixture]
public class AudioSamplingRateOptionViewModelTests
{
    [Test]
    public void New_AudioSamplingRateOptionViewModel_After_Initialization()
    {
        AudioSamplingRateOptionViewModel fixture = InitializeFixture();

        fixture.SamplingRate.Should().Be(DefaultSamplingRate);
        fixture.HasChanged.Should().BeFalse();
        fixture.DefaultOptions.Length.Should().Be(3);
    }

    [Test]
    public void After_value_changed_HasChanged_should_be_true()
    {
        const int newSamplingRate = 48000;
        AudioSamplingRateOptionViewModel fixture = InitializeFixture();

        fixture.SamplingRate = newSamplingRate;

        fixture.HasChanged.Should().BeTrue();
        fixture.SamplingRate.Should().Be(newSamplingRate);
    }

    [Test]
    [TestCase(0, 44100)]
    [TestCase(1, 48000)]
    [TestCase(2, 96000)]
    public void Executing_ValuesUpdater_should_update_value(int index, int bitrate)
    {
        AudioSamplingRateOptionViewModel fixture = InitializeFixture(0);
        ValuesUpdater updater = fixture.DefaultOptions[index];

        updater.Update.Invoke();

        fixture.SamplingRate.Should().Be(bitrate);
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void After_set_initial_value_HasChanged_should_be_false()
    {
        AudioSamplingRateOptionViewModel fixture = InitializeFixture();

        fixture.SamplingRate = 48000;
        fixture.SamplingRate = DefaultSamplingRate;

        fixture.HasChanged.Should().BeFalse();
        fixture.SamplingRate.Should().Be(DefaultSamplingRate);
    }


    private static AudioSamplingRateOptionViewModel InitializeFixture(int? initialSamplingRate = null)
    {
        ISchedulerProvider scheduler = new ImmediateSchedulers();
        AudioSamplingRateOption option = new(initialSamplingRate ?? DefaultSamplingRate);
        return new AudioSamplingRateOptionViewModel(option, scheduler);
    }

    private const int DefaultSamplingRate = 44100;
}

