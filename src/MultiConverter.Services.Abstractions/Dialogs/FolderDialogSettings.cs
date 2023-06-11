namespace MultiConverter.Services.Abstractions.Dialogs;

public record FolderDialogSettings(bool AllowMultiple, string Title, string? Directory = null) : DialogSettingsBase(Title, Directory);
