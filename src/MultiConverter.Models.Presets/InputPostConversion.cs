using MultiConverter.Models.Presets.Enums;

namespace MultiConverter.Models.Presets;

public record InputPostConversion(InputPostConversionAction PostConversionAction, string ArchiveFolderPath,
    bool IncludeProcessingDate, bool KeepAbsolutePath)
{
    public static InputPostConversion Default { get; } = new(
        InputPostConversionAction.None,
        string.Empty,
        false,
        false
    );
}
