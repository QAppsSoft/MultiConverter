using FluentAssertions;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Configurations;
using MultiConverter.Models.Presets.Options;
using MultiConverter.Services.Formats;
using MultiConverter.ViewModels.Presets.Options;

namespace MultiConverter.ViewModelsFixtures.Presets.Options;

[TestFixture]
public class VideoCodecOptionViewModelTests
{
    private const string DefaultCodecName = "mpeg4";


    [Test]
    public void New_VideoCodecOptionViewModel_After_Initialization()
    {
        VideoCodecOptionViewModel fixture = InitializeFixture();

        fixture.SelectedCodec.Name.Should().Be(DefaultCodecName);
        fixture.HasChanged.Should().BeFalse();
        fixture.DefaultOptions.Length.Should().Be(4);
    }

    [Test]
    public void After_value_changed_HasChanged_should_be_true()
    {
        const string newCodecName = "libxvid";
        VideoCodecOptionViewModel fixture = InitializeFixture();

        fixture.SelectedCodec = fixture.Codecs.First(c => c.Name == newCodecName);

        fixture.HasChanged.Should().BeTrue();
        fixture.SelectedCodec.Name.Should().Be(newCodecName);
    }

    [Test]
    [TestCase(0, "mpeg1video")]
    [TestCase(1, "mpeg2video")]
    [TestCase(2, "mpeg4")]
    [TestCase(3, "libxvid")]
    public void Executing_ValuesUpdater_should_update_value(int index, string codecName)
    {
        VideoCodecOptionViewModel fixture = InitializeFixture("h263");
        ValuesUpdater updater = fixture.DefaultOptions[index];

        updater.Update.Invoke();

        fixture.SelectedCodec.Name.Should().Be(codecName);
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void After_set_initial_value_HasChanged_should_be_false()
    {
        VideoCodecOptionViewModel fixture = InitializeFixture();

        fixture.SelectedCodec = fixture.Codecs.First(c => c.Name == "libxvid");
        fixture.SelectedCodec = fixture.Codecs.First(c => c.Name == DefaultCodecName);

        fixture.HasChanged.Should().BeFalse();
        fixture.SelectedCodec.Name.Should().Be(DefaultCodecName);
    }

    private static VideoCodecOptionViewModel InitializeFixture(string? initialCodec = null)
    {
        ISchedulerProvider scheduler = new ImmediateSchedulers();
        VideoCodecOption option = new(initialCodec ?? DefaultCodecName);
        FavoriteCodecsConfiguration configuration = new() { Favorites = new List<string>() };
        CodecsProvider provider = new(configuration);
        return new VideoCodecOptionViewModel(option, provider,scheduler);
    }
}
