using System.Threading.Tasks;

namespace MultiConverter.Services;

public interface IDialogService
{
    Task<string?> ShowFolderSelectorAsync(FolderDialogSettings? options = null);
}
