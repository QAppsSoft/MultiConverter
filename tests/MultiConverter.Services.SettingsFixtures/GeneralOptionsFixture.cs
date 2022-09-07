using System.IO;
using FluentAssertions;
using MultiConverter.Models.Settings;
using MultiConverter.Models.Settings.General;
using NUnit.Framework;

namespace MultiConverter.Services.SettingsFixtures;

public class GeneralOptionsFixture
{
    [Test]
    public void Create_default_GeneralOptions()
    {
        string[] supportedFilesExtensions =
        {
            ".264", ".3g2", ".3gp", ".amv", ".asf", ".avi", ".avs", ".bik", ".cpk", ".dat", ".dif", ".divx", ".dv",
            ".dvr-ms", ".f4v", ".fli", ".flv", ".h264", ".m2ts", ".m4v", ".mkv", ".mkv", ".mod", ".mov", ".mp4",
            ".mp4", ".mpeg", ".mpg", ".mts", ".ogm", ".qt", ".rm", ".rmvb", ".ts", ".vfw", ".vob", ".webm", ".wmv",
            ".xv", ".yuv"
        };

        GeneralOptions defaultOptions = GeneralOptions.Default();

        defaultOptions.Theme.Should().Be(Theme.Dark);
        defaultOptions.Language.Should().Be("en");
        defaultOptions.AnalysisTimeout.Should().Be(60);
        defaultOptions.TemporalFolder.Should().Be(Path.GetTempPath());
        defaultOptions.SupportedFilesExtensions.Should().BeEquivalentTo(supportedFilesExtensions);
        defaultOptions.FileFilters.Should().BeEmpty();
        defaultOptions.LoadFilesAlreadyInQueue.Should().Be(false);
    }
}
