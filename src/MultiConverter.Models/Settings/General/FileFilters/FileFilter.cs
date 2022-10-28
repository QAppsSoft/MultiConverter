namespace MultiConverter.Models.Settings.General.FileFilters;

public readonly record struct FileFilter(string Filter, FileFilterPosition Position, FileFilterApplyOn ApplyOn)
{
    public static FileFilter Default { get; } = new("", FileFilterPosition.Contains, FileFilterApplyOn.FileAndExtension);
}
