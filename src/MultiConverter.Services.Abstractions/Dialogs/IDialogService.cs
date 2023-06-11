using System.Threading.Tasks;

namespace MultiConverter.Services.Abstractions.Dialogs;

public interface IDialogService
{
    Task<string[]> ShowFolderSelectorAsync(FolderDialogSettings? options = null);

    Task<string?> ShowSaveFileDialogSelectorAsync(SaveFileDialogSettings? options = null);

    Task<string[]> ShowOpenFileDialogSelectorAsync(OpenFileDialogSettings? options = null);
}
