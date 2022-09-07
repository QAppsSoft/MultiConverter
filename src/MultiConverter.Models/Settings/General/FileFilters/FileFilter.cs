namespace MultiConverter.Models.Settings.General.FileFilters;

public readonly record struct FileFilter(string Filter, FileFilterPosition Position, FileFilterApplyOn ApplyOn);
