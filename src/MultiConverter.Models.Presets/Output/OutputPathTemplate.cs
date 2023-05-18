using MultiConverter.Models.Presets.Enums;

namespace MultiConverter.Models.Presets.Output;

public record OutputPathTemplate(OutputPathSelection OutputPathSelection, string Template, bool OverrideContainerExtension, string OutputExtension, string FixedPath, string AddInPathCollision)
{
    public static OutputPathTemplate Default { get; } =
        new(OutputPathSelection.SameAsInput, "(P)(f)", false, string.Empty, string.Empty, "-");
}
