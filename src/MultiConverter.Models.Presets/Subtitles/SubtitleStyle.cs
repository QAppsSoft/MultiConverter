using MultiConverter.Models.Presets.Enums;
using MultiConverter.Models.Presets.Extensions;

namespace MultiConverter.Models.Presets.Subtitles;

/// <summary>
///     Styles define the appearance and position of subtitles
/// </summary>
/// <param name="FontName">The fontname as used by Windows. Case-sensitive.</param>
/// <param name="FontSize">Fontsize</param>
/// <param name="PrimaryColour">This is the colour that a subtitle will normally appear in.</param>
/// <param name="SecondaryColour">This colour may be used instead of the Primary colour when a subtitle is automatically shifted to prevent an onscreen collision, to distinguish the different subtitles.</param>
/// <param name="OutlineColour">This colour may be used instead of the Primary or Secondary colour when a subtitle is automatically shifted to prevent an onscreen collision, to distinguish the different subtitles.</param>
/// <param name="BackColour">This is the colour of the subtitle outline or shadow, if these are used.</param>
/// <param name="Bold">This defines whether text is bold.</param>
/// <param name="Italic">This defines whether text is italic.</param>
/// <param name="Underline">This defines whether text is underlined.</param>
/// <param name="StrikeOut">This defines whether text is strikeout.</param>
/// <param name="ScaleX">Modifies the width of the font. (percent)</param>
/// <param name="ScaleY">Modifies the height of the font. (percent)</param>
/// <param name="Spacing">Extra space between characters. (pixels)</param>
/// <param name="Angle">The origin of the rotation is defined by the alignment. (degrees)</param>
/// <param name="BorderStyle">Border style <see cref="BorderStyle"/></param>
/// <param name="Outline">If BorderStyle is Outline, then this specifies the width of the outline around the text, in pixels. Values may be 0, 1, 2, 3 or 4.</param>
/// <param name="Shadow">If BorderStyle is Outline, then this specifies the depth of the drop shadow behind the text, in pixels. Values may be 0, 1, 2, 3 or 4. Drop shadow is always used in addition to an outline - SSA will force an outline of 1 pixel if no outline width is given.</param>
/// <param name="Alignment">This sets how text is "justified" within the Left/Right onscreen margins, and also the vertical placing. <see cref="Alignment"/></param>
/// <param name="MarginL">This defines the Left Margin in pixels. It is the distance from the left-hand edge of the screen.The three onscreen margins (MarginL, MarginR, MarginV) define areas in which the subtitle text will be displayed.</param>
/// <param name="MarginR">This defines the Right Margin in pixels. It is the distance from the right-hand edge of the screen. The three onscreen margins (MarginL, MarginR, MarginV) define areas in which the subtitle text will be displayed.</param>
/// <param name="MarginV">This defines the vertical Left Margin in pixels. For a subtitle, it is the distance from the bottom of the screen. For a toptitle, it is the distance from the top of the screen. For a midtitle, the value is ignored - the text will be vertically centered
/// </param>
public record SubtitleStyle(string FontName, int FontSize, AssColor PrimaryColour, AssColor SecondaryColour, AssColor OutlineColour,
    AssColor BackColour, bool Bold, bool Italic, bool Underline, bool StrikeOut, int ScaleX, int ScaleY, int Spacing, int Angle,
    SubtitleBorderStyle BorderStyle, int Outline, int Shadow, SubtitleAlignment Alignment, int MarginL, int MarginR, int MarginV)
{
    public Dictionary<string, string> ToDictionary() => new()
    {
        { "FontName", FontName },
        { "FontSize", FontSize.ToString() },
        { "PrimaryColour", PrimaryColour.ToAssColorString() },
        { "SecondaryColour", SecondaryColour.ToAssColorString() },
        { "OutlineColour", OutlineColour.ToAssColorString() },
        { "BackColour", BackColour.ToAssColorString() },
        { "Bold", Bold  ? "-1" : "0" },
        { "Italic", Italic  ? "-1" : "0"  },
        { "Underline", Underline  ? "-1" : "0"  },
        { "Strikeout", StrikeOut ? "-1" : "0" },
        { "ScaleX", ScaleX.ToString() },
        { "ScaleY", ScaleY.ToString() },
        { "Spacing", Spacing.ToString() },
        { "Angle", Angle.ToString() },
        { "BorderStyle", Convert.ChangeType(BorderStyle, BorderStyle.GetTypeCode()).ToString()! }, // ((int) BorderStyle).ToString() },
        { "Outline", Outline.ToString() },
        { "Shadow", Shadow.ToString() },
        { "Alignment", Convert.ChangeType(Alignment, Alignment.GetTypeCode()).ToString()! }, // ((int) Alignment).ToString() },
        { "MarginL", MarginL.ToString() },
        { "MarginR", MarginR.ToString() },
        { "MarginV", MarginV.ToString() }
    };

    public static SubtitleStyle Default { get; } = new(
        "Arial",
        16,
        new AssColor("FF", "FF", "FF", "FF"),
        new AssColor("FF", "FF", "FF", "FF"),
        new AssColor("00", "00", "00", "00"),
        new AssColor("00", "00", "00", "00"),
        false,
        false,
        false,
        false,
        100,
        100,
        0,
        0,
        SubtitleBorderStyle.Outline,
        1,
        0,
        SubtitleAlignment.Centered,
        10,
        10,
        10
    );
}
