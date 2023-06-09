using System.Threading.Tasks;

namespace MultiConverter.Services.Abstractions.Dialogs;

public interface IDialogService
{
    Task<string?> ShowFolderSelectorAsync(FolderDialogSettings? options = null);
}
