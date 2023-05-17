namespace MultiConverter.Models.Presets.Subtitles;

public record AssColor(string Alpha, string Red, string Green, string Blue)
{
    public static AssColor Default { get; } = new("FF", "FF", "FF", "FF");
}
