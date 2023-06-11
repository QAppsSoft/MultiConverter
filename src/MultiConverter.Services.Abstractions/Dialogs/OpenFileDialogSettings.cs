namespace MultiConverter.Services.Abstractions.Dialogs;

public record OpenFileDialogSettings(bool AllowMultiple, FileDialogExtensions[] Extensions, string Title, string? Directory = null) : DialogSettingsBase(Title, Directory);
