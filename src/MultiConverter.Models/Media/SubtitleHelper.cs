using System.IO;
using System.Linq;

namespace MultiConverter.Models.Media;

internal static class SubtitleHelper
{
    private static readonly string[] s_externalSubtitles = { ".srt", ".ssa", ".ass", ".vtt", ".sub" };

    private static readonly SubtitleType[] s_canBurnInSubtitles =
    {
        SubtitleType.SRT, SubtitleType.SSA, SubtitleType.ASS, SubtitleType.SUBRIP, SubtitleType.WebVTT,
        SubtitleType.MicroDVD
    };

    public static bool IsExternal(string subtitlePath) => IsExternal(new FileInfo(subtitlePath));

    public static bool IsExternal(FileSystemInfo subtitle) => s_externalSubtitles.Contains(subtitle.Extension);

    public static bool CheckIfCanBurnIn(SubtitleType subtitleType) => s_canBurnInSubtitles.Contains(subtitleType);
}
