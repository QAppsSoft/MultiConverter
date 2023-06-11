using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using MultiConverter.Services.Abstractions.Dialogs;

namespace MultiConverter.Services.Implementations;

public class DialogService : IDialogService
{
    private static readonly IStorageProvider? s_storageProvider = GetStorageProvider();

    public async Task<string[]> ShowFolderSelectorAsync(FolderDialogSettings? options = null)
    {
        FolderPickerOpenOptions pickerOptions = new();

        if (options is null)
        {
            pickerOptions.AllowMultiple = false;
        }
        else
        {
            pickerOptions.Title = options.Title;
            pickerOptions.AllowMultiple = options.AllowMultiple;

            await SetStartLocation(pickerOptions, options);
        }

        Uri[] result = await OpenFolderPickerAsync(pickerOptions);

        return result.Select(uri => uri.LocalPath).ToArray();
    }

    public async Task<string?> ShowSaveFileDialogSelectorAsync(SaveFileDialogSettings? options = null)
    {
        FilePickerSaveOptions pickerOptions = new();

        if (options is not null)
        {
            pickerOptions.Title = options.Title;
            pickerOptions.SuggestedFileName = options.SuggestedFileName;
            pickerOptions.ShowOverwritePrompt = !options.Overwrite;
            pickerOptions.FileTypeChoices = options.Extensions
                .Select(x => new FilePickerFileType(x.Name) { Patterns = x.Extensions }).ToArray();

            await SetStartLocation(pickerOptions, options);
        }

        Uri? result = await SaveFilePickerAsync(pickerOptions);

        return result?.LocalPath;
    }

    public async Task<string[]> ShowOpenFileDialogSelectorAsync(OpenFileDialogSettings? options = null)
    {
        FilePickerOpenOptions pickerOptions = new();

        if (options is null)
        {
            pickerOptions.AllowMultiple = false;
        }
        else
        {
            pickerOptions.Title = options.Title;
            pickerOptions.AllowMultiple = options.AllowMultiple;
            pickerOptions.FileTypeFilter = options.Extensions
                .Select(x => new FilePickerFileType(x.Name) { Patterns = x.Extensions }).ToArray();

            await SetStartLocation(pickerOptions, options);
        }

        Uri[] result = await OpenFilePickerAsync(pickerOptions);

        return result.Select(uri => uri.LocalPath).ToArray();
    }

    private static async Task<Uri[]> OpenFolderPickerAsync(FolderPickerOpenOptions options)
    {
        if (s_storageProvider is null)
        {
            return Array.Empty<Uri>();
        }

        var pickerResult = await s_storageProvider.OpenFolderPickerAsync(options);

        return pickerResult.Select(x => x.Path).ToArray();
    }

    private static async Task<Uri[]> OpenFilePickerAsync(FilePickerOpenOptions options)
    {
        if (s_storageProvider is null)
        {
            return Array.Empty<Uri>();
        }

        var pickerResult = await s_storageProvider.OpenFilePickerAsync(options);

        return pickerResult.Select(p => p.Path).ToArray();
    }

    private static async Task<Uri?> SaveFilePickerAsync(FilePickerSaveOptions options)
    {
        if (s_storageProvider is null)
        {
            return null;
        }

        var pickerResult = await s_storageProvider.SaveFilePickerAsync(options);

        return pickerResult?.Path;
    }

    private static async Task SetStartLocation(PickerOptions pickerOptions, DialogSettingsBase settingsBase)
    {
        if (await GetStorageFolder(settingsBase) is { } storageFolder)
        {
            pickerOptions.SuggestedStartLocation = storageFolder;
        }
    }

    private static async Task<IStorageFolder?> GetStorageFolder(DialogSettingsBase settingsBase)
    {
        if (settingsBase.Directory is not null && s_storageProvider is not null)
        {
            return await s_storageProvider.TryGetFolderFromPathAsync(settingsBase.Directory);
        }

        return null;
    }

    private static IStorageProvider? GetStorageProvider() => GetMainWindow()?.StorageProvider;

    private static Window? GetMainWindow()
    {
        var lifetime = Avalonia.Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        return lifetime?.MainWindow;
    }
}
