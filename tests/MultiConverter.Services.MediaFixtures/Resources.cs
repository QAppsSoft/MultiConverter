using System;
using System.IO;

namespace MultiConverter.Services.MediaFixtures;

internal static class Resources
{
    internal static readonly string Mp3 = GetResourceFilePath("audio.mp3");
    internal static readonly string MkvWithSubtitles = GetResourceFilePath("mkvWithSubtitles.mkv");
    internal static readonly string SubtitleSrt = GetResourceFilePath("sampleSrt.srt");
    internal static string GetResourceFilePath(string fileName) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", fileName);
}
