namespace MultiConverter.Services.Abstractions.Dialogs;

public class FolderDialogSettings
{
    public FolderDialogSettings(string? title = null, string? directory = null)
    {
        Title = title;
        Directory = directory;
    }

    public string? Title { get; }

    public string? Directory { get; }

    public static FolderDialogSettings Default { get; } = new();
}
