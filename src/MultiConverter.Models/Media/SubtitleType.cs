using System.ComponentModel;

namespace MultiConverter.Models.Media;

/// <summary>
///     Subtitle Type.
/// </summary>
public enum SubtitleType
{
    [Description("SSA")] SSA,

    [Description("ASS")] ASS,

    [Description("SRT")] SRT,

    [Description("VobSub")] VobSub,

    [Description("CC")] CC,

    [Description("UTF8")] UTF8Sub,

    [Description("TX3G")] TX3G,

    [Description("PGS")] PGS,

    [Description("SUBRIP")] SUBRIP,

    [Description("WebVTT")] WebVTT,

    [Description("MicroDVD")] MicroDVD,

    [Description("Unknown")] Unknown
}
