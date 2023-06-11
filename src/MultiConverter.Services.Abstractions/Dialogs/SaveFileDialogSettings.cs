namespace MultiConverter.Services.Abstractions.Dialogs;

public record SaveFileDialogSettings(FileDialogExtensions[] Extensions, bool Overwrite, string SuggestedFileName, string Title, string? Directory = null) : DialogSettingsBase(Title, Directory);
