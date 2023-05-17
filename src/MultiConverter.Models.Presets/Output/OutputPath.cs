namespace MultiConverter.Models.Presets.Output;

public record OutputPathTemplate(string Template, bool OverrideContainerExtension, string OutputExtension)
{
    public static OutputPathTemplate Default { get; } = new("(P)(f)", false, string.Empty);
}
