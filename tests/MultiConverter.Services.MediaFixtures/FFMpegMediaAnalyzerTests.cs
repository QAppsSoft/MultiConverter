using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MultiConverter.Models.Media;
using MultiConverter.Services.Abstractions.Media;
using MultiConverter.Services.Media;
using NUnit.Framework;

namespace MultiConverter.Services.MediaFixtures;

public class FFMpegMediaAnalyzerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Multiple_streams_file_container_creation()
    {
        FFMpegMediaAnalyzer analyzer = new();
        IContainer container = await analyzer.AnalyzeAsync(Resources.MkvWithSubtitles);
        IAudioStream audioTrack = container.AudioTracks.First();
        ISubtitleStream[] subtitles = container.Subtitles.ToArray();

        // Container tests
        container.Duration.Should().Be(new TimeSpan(0, 1, 29, 37, 878));

        // Video tests
        container.Video.Should().NotBeNull();
        container.Video.Format.Should().Be("h264");
        container.Video.FrameRate.Should().Be(25);
        container.Video.Size.IsEmpty.Should().BeFalse();
        container.Video.Size.Width.Should().Be(320);
        container.Video.Size.Height.Should().Be(240);
        container.Video.AutoCropDimensions.Should().Be(Cropping.NoCrop());

        // Audio tests
        container.AudioTracks.Count().Should().Be(1);

        audioTrack.BitRate.Should().Be(0);
        audioTrack.Format.Should().Be("aac");
        audioTrack.IsDefault.Should().BeTrue();
        audioTrack.IsForced.Should().BeFalse();
        audioTrack.SampleRate.Should().Be(48000);

        // Subtitle tests
        container.Subtitles.Count().Should().Be(2);

        subtitles[0].Index.Should().Be(2);
        subtitles[0].StreamIndex.Should().Be(0);
        subtitles[0].CanBurnIn.Should().BeTrue();
        subtitles[0].IsDefault.Should().BeFalse();
        subtitles[0].IsForced.Should().BeFalse();
        subtitles[0].Language.Should().BeEmpty();
        subtitles[0].LanguageCode.Should().Be("ger");
        subtitles[0].SubtitleType.Should().Be(SubtitleType.SUBRIP);
        subtitles[0].Title.Should().BeEmpty();
        subtitles[0].IsExternalSubtitle.Should().BeFalse();

        subtitles[1].Index.Should().Be(3);
        subtitles[1].StreamIndex.Should().Be(1);
        subtitles[1].CanBurnIn.Should().BeTrue();
        subtitles[1].IsDefault.Should().BeFalse();
        subtitles[1].IsForced.Should().BeFalse();
        subtitles[1].Language.Should().BeEmpty();
        subtitles[1].LanguageCode.Should().Be("eng");
        subtitles[1].SubtitleType.Should().Be(SubtitleType.SUBRIP);
        subtitles[1].Title.Should().BeEmpty();
        subtitles[1].IsExternalSubtitle.Should().BeFalse();
    }

    [Test]
    public async Task Subtitle_file_container_creation()
    {
        FFMpegMediaAnalyzer analyzer = new();
        var options = new AnalyzerOptions { Timeout = new TimeSpan(0, 0, 6, 0) };
        IContainer container = await analyzer.AnalyzeAsync(Resources.SubtitleSrt, options);
        ISubtitleStream[] subtitles = container.Subtitles.ToArray();

        // Container tests
        container.Duration.Should().Be(new TimeSpan(0, 0, 0, 0, 0));

        // Video tests
        container.Video.Should().BeNull();

        // Audio tests
        container.AudioTracks.Count().Should().Be(0);

        // Subtitle tests
        container.Subtitles.Count().Should().Be(1);

        subtitles[0].Index.Should().Be(0);
        subtitles[0].StreamIndex.Should().Be(0);
        subtitles[0].CanBurnIn.Should().BeTrue();
        subtitles[0].IsDefault.Should().BeFalse();
        subtitles[0].IsForced.Should().BeFalse();
        subtitles[0].Language.Should().BeEmpty();
        subtitles[0].LanguageCode.Should().BeEmpty();
        subtitles[0].SubtitleType.Should().Be(SubtitleType.SUBRIP);
        subtitles[0].Title.Should().BeEmpty();
        subtitles[0].IsExternalSubtitle.Should().BeTrue();
    }
}
