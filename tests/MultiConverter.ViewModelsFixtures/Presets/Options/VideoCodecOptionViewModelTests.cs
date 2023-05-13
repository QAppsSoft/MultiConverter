using FluentAssertions;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Presets.Options;
using MultiConverter.ViewModels.Presets.Options;

namespace MultiConverter.ViewModelsFixtures.Presets.Options;

[TestFixture]
public class VideoCodecOptionViewModelTests
{
    private const string DefaultCodec = "mpeg4";


    [Test]
    public void New_VideoCodecOptionViewModel_After_Initialization()
    {
        VideoCodecOptionViewModel fixture = InitializeFixture();

        fixture.Codec.Should().Be(DefaultCodec);
        fixture.HasChanged.Should().BeFalse();
        fixture.DefaultOptions.Length.Should().Be(4);
    }

    [Test]
    public void After_value_changed_HasChanged_should_be_true()
    {
        const string newCodec = "libxvid";
        VideoCodecOptionViewModel fixture = InitializeFixture();

        fixture.Codec = newCodec;

        fixture.HasChanged.Should().BeTrue();
        fixture.Codec.Should().Be(newCodec);
    }

    [Test]
    [TestCase(0, "mpeg1video")]
    [TestCase(1, "mpeg2video")]
    [TestCase(2, "mpeg4")]
    [TestCase(3, "libxvid")]
    public void Executing_ValuesUpdater_should_update_value(int index, string codec)
    {
        VideoCodecOptionViewModel fixture = InitializeFixture(VideoCodecOption.Default);
        ValuesUpdater updater = fixture.DefaultOptions[index];

        updater.Update.Invoke();

        fixture.Codec.Should().Be(codec);
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void After_set_initial_value_HasChanged_should_be_false()
    {
        VideoCodecOptionViewModel fixture = InitializeFixture();

        fixture.Codec = "VP9";
        fixture.Codec = DefaultCodec;

        fixture.HasChanged.Should().BeFalse();
        fixture.Codec.Should().Be(DefaultCodec);
    }

    private static VideoCodecOptionViewModel InitializeFixture(string? initialCodec = null)
    {
        ISchedulerProvider scheduler = new ImmediateSchedulers();
        VideoCodecOption option = new(initialCodec ?? DefaultCodec);
        return new VideoCodecOptionViewModel(option, scheduler);
    }
}
