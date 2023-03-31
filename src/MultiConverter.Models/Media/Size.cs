using System;
using System.Collections.Generic;

namespace MultiConverter.Models.Media;

/// <summary>
///     The size.
/// </summary>
public readonly record struct Size(int Width, int Height)
{
    /// <summary> Empty </summary>
    public static readonly Size Empty = new(0, 0);

private readonly string _resolution = $"{Width}x{Height}";

/// <summary>1408x1152</summary>
public static Size _16cif => new(1408, 1152);

/// <summary>2048x1080</summary>
public static Size _2K => new(2048, 1080);

/// <summary>2048x1080</summary>
public static Size _2Kdci => new(2048, 1080);

/// <summary>1998x1080</summary>
public static Size _2Kflat => new(1998, 1080);

/// <summary>2048x858</summary>
public static Size _2Kscope => new(2048, 858);

/// <summary>704x576</summary>
public static Size _4Cif => new(704, 576);

/// <summary>4096x2160</summary>
public static Size _4K => new(4096, 2160);

/// <summary>4096x2160</summary>
public static Size _4Kdci => new(4096, 2160);

/// <summary>3996x2160</summary>
public static Size _4Kflat => new(3996, 2160);

/// <summary>4096x1716</summary>
public static Size _4Kscope => new(4096, 1716);

/// <summary>320x200</summary>
public static Size Cga => new(320, 200);

/// <summary>352x288</summary>
public static Size Cif => new(352, 288);

/// <summary>640x350</summary>
public static Size Ega => new(640, 350);

/// <summary>352x240</summary>
public static Size Film => new(352, 240);

/// <summary>432x240</summary>
public static Size Fwqvga => new(432, 240);

/// <summary>1920x1080</summary>
public static Size Hd1080 => new(1920, 1080);

/// <summary>852x480</summary>
public static Size Hd480 => new(852, 480);

/// <summary>1280x720</summary>
public static Size Hd720 => new(1280, 720);

/// <summary>240x160</summary>
public static Size Hqvga => new(240, 160);

/// <summary>5120x4096</summary>
public static Size Hsxga => new(5120, 4096);

/// <summary>480x320</summary>
public static Size Hvga => new(480, 320);

/// <summary>640x360</summary>
public static Size Nhd => new(640, 360);

/// <summary>720x480</summary>
public static Size Ntsc => new(720, 480);

/// <summary>352x240</summary>
public static Size NtscFilm => new(352, 240);

/// <summary>720x576</summary>
public static Size Pal => new(720, 576);

/// <summary>176x144</summary>
public static Size Qcif => new(176, 144);

/// <summary>960x540</summary>
public static Size Qhd => new(960, 540);

/// <summary>352x240</summary>
public static Size Qntsc => new(352, 240);

/// <summary>352x288</summary>
public static Size Qpal => new(352, 288);

/// <summary>160x120</summary>
public static Size Qqvga => new(160, 120);

/// <summary>2560x2048</summary>
public static Size Qsxga => new(2560, 2048);

/// <summary>320x240</summary>
public static Size Qvga => new(320, 240);

/// <summary>2048x1536</summary>
public static Size Qxga => new(2048, 1536);


public static IEnumerable<Size> Sizes => new[]
{
        SVCD, Ntsc, Qntsc, Qvga, Vga, Svga, Xga, Uxga, Hd480, Hd720, Hd1080
    };

/// <summary>640x480</summary>
public static Size Sntsc => new(640, 480);

/// <summary>768x576</summary>
public static Size Spal => new(768, 576);

/// <summary>128x96</summary>
public static Size Sqcif => new(128, 96);

/// <summary>480x480</summary>
public static Size SVCD => new(480, 480);

/// <summary>800x600</summary>
public static Size Svga => new(800, 600);

/// <summary>1280x1024</summary>
public static Size Sxga => new(1280, 1024);

/// <summary>3840x2160</summary>
public static Size Uhd2160 => new(3840, 2160);

/// <summary>7680x4320</summary>
public static Size Uhd4320 => new(7680, 4320);

/// <summary>1600x1200</summary>
public static Size Uxga => new(1600, 1200);

/// <summary>640x480</summary>
public static Size Vga => new(640, 480);

/// <summary>6400x4096</summary>
public static Size Whsxga => new(6400, 4096);

/// <summary>7680x4800</summary>
public static Size Whuxga => new(7680, 4800);

/// <summary>2560x1600</summary>
public static Size Woxga => new(2560, 1600);

/// <summary>3200x2048</summary>
public static Size Wqsxga => new(3200, 2048);

/// <summary>3840x2400</summary>
public static Size Wquxga => new(3840, 2400);

/// <summary>400x240</summary>
public static Size Wqvga => new(400, 240);

/// <summary>1600x1024</summary>
public static Size Wsxga => new(1600, 1024);

/// <summary>1920x1200</summary>
public static Size Wuxga => new(1920, 1200);

/// <summary>852x480</summary>
public static Size Wvga => new(852, 480);

/// <summary>1366x768</summary>
public static Size Wxga => new(1366, 768);

/// <summary>1024x768</summary>
public static Size Xga => new(1024, 768);

/// <summary>
///     Gets a value indicating whether is empty.
/// </summary>
public bool IsEmpty => Width <= 0 && Height <= 0;

public static Size Parse(string resolution)
{
    if (string.IsNullOrWhiteSpace(resolution))
    {
        throw new ArgumentNullException(nameof(resolution));
    }

    string[] values = resolution.Split('x');
    if (values.Length != 2)
    {
        throw new ApplicationException(
            "The resolution formatted string must contains two integers with an 'x' in the middle");
    }

    return new Size(int.Parse(values[0]), int.Parse(values[1]));
}

public static bool TryParse(string resolution, out Size? size)
{
    if (string.IsNullOrWhiteSpace(resolution))
    {
        throw new ArgumentNullException(nameof(resolution));
    }

    size = null;

    string[] values = resolution.Split('x');
    if (values.Length != 2)
    {
        throw new ApplicationException(
            "The resolution formatted string must contains two integers with an 'x' in the middle");
    }

    bool tryWidth = int.TryParse(values[0], out int width);
    bool tryHeight = int.TryParse(values[1], out int height);

    if (tryHeight && tryWidth)
    {
        size = new Size(width, height);
        return true;
    }

    return false;
}

public override string ToString() => _resolution;
}
