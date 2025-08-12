using AwesomeAssertions;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Configurations;
using MultiConverter.Models.Presets.Options;
using MultiConverter.Services.Formats;
using MultiConverter.ViewModels.Presets.Options;

namespace MultiConverter.ViewModelsFixtures.Presets.Options;

[TestFixture]
public class AudioCodecOptionViewModelTests
{
    private const string DefaultCodec = "flac";

    [Test]
    public void New_VideoFrameRateOptionViewModel_After_Initialization()
    {
        AudioCodecOptionViewModel fixture = InitializeFixture();

        fixture.SelectedCodec.Name.Should().Be(DefaultCodec);
        fixture.HasChanged.Should().BeFalse();
        fixture.DefaultOptions.Length.Should().Be(3);
    }

    [Test]
    public void After_value_changed_HasChanged_should_be_true()
    {
        const string newCodec = "mp2";
        AudioCodecOptionViewModel fixture = InitializeFixture();

        fixture.SelectedCodec = fixture.Codecs.First(c => c.Name == newCodec);

        fixture.HasChanged.Should().BeTrue();
        fixture.SelectedCodec.Name.Should().Be(newCodec);
    }

    [Test]
    [TestCase(0, "mp2")]
    [TestCase(1, "libmp3lame")]
    [TestCase(2, "aac")]
    public void Executing_ValuesUpdater_should_update_value(int index, string codec)
    {
        AudioCodecOptionViewModel fixture = InitializeFixture(DefaultCodec);
        ValuesUpdater updater = fixture.DefaultOptions[index];

        updater.Update.Invoke();

        fixture.SelectedCodec.Name.Should().Be(codec);
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void After_set_initial_value_HasChanged_should_be_false()
    {
        AudioCodecOptionViewModel fixture = InitializeFixture();

        fixture.SelectedCodec = fixture.Codecs.First(c => c.Name == "aac");
        fixture.SelectedCodec = fixture.Codecs.First(c => c.Name == DefaultCodec);

        fixture.HasChanged.Should().BeFalse();
        fixture.SelectedCodec.Name.Should().Be(DefaultCodec);
    }

    private static AudioCodecOptionViewModel InitializeFixture(string? initialCodec = null)
    {
        ISchedulerProvider scheduler = new ImmediateSchedulers();
        AudioCodecOption option = new(initialCodec ?? DefaultCodec);
        FavoriteCodecsConfiguration configuration = new() { Favorites = new List<string>() };
        CodecsProvider codecsProvider = new(configuration);
        return new AudioCodecOptionViewModel(option, codecsProvider, scheduler);
    }
}
