using FluentAssertions;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Presets.Options;
using MultiConverter.ViewModels.Presets.Options;

namespace MultiConverter.ViewModelsFixtures.Presets.Options;

[TestFixture]
public class AudioCodecOptionViewModelTests
{
    [Test]
    public void New_VideoFrameRateOptionViewModel_After_Initialization()
    {
        AudioCodecOptionViewModel fixture = InitializeFixture();

        fixture.AudioCodec.Should().Be(DefaultCodec);
        fixture.HasChanged.Should().BeFalse();
        fixture.DefaultOptions.Length.Should().Be(4);
    }

    [Test]
    public void After_value_changed_HasChanged_should_be_true()
    {
        const string newCodec = "mp2";
        AudioCodecOptionViewModel fixture = InitializeFixture();

        fixture.AudioCodec = newCodec;

        fixture.HasChanged.Should().BeTrue();
        fixture.AudioCodec.Should().Be(newCodec);
    }

    [Test]
    [TestCase(0, "MP1")]
    [TestCase(1, "MP2")]
    [TestCase(2, "MP3")]
    [TestCase(3, "AAC")]
    public void Executing_ValuesUpdater_should_update_value(int index, string codec)
    {
        AudioCodecOptionViewModel fixture = InitializeFixture(AudioCodecOption.Default);
        ValuesUpdater updater = fixture.DefaultOptions[index];

        updater.Update.Invoke();

        fixture.AudioCodec.Should().Be(codec);
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void After_set_initial_value_HasChanged_should_be_false()
    {
        AudioCodecOptionViewModel fixture = InitializeFixture();

        fixture.AudioCodec = "AAC";
        fixture.AudioCodec = DefaultCodec;

        fixture.HasChanged.Should().BeFalse();
        fixture.AudioCodec.Should().Be(DefaultCodec);
    }


    private static AudioCodecOptionViewModel InitializeFixture(string? initialCodec = null)
    {
        ISchedulerProvider scheduler = new ImmediateSchedulers();
        AudioCodecOption option = new(initialCodec ?? DefaultCodec);
        return new AudioCodecOptionViewModel(option, scheduler);
    }

    private const string DefaultCodec = "libmp3lame ";
}
