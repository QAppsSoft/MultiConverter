using System.Threading.Tasks;
using Avalonia.Controls;
using MultiConverter.Services.Abstractions.Dialogs;

namespace MultiConverter.Services.Implementations;

public class DialogService : IDialogService
{
    public Task<string?> ShowFolderSelectorAsync(FolderDialogSettings? options = null)
    {
        OpenFolderDialog dialog = new();

        if (options != null)
        {
            dialog.Title = options.Title;
            dialog.Directory = options.Directory;
        }

        return dialog.ShowAsync(App.MainWindow);
    }
}
