using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using MultiConverter.ViewModels.Presets;
using ReactiveUI;

namespace MultiConverter.Views.Presets;

public partial class PresetsContainerView : ReactiveUserControl<PresetsContainerViewModel>
{
    public PresetsContainerView()
    {
        InitializeComponent();

        //StorageProvider = App.MainWindow.StorageProvider;

        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            StorageProvider = desktopLifetime.MainWindow.StorageProvider;
        }

        this.WhenActivated(disposable =>
        {
            RegisterSaveFileDialog();
            RegisterOpenFileDialog();
        });
    }

    public IStorageProvider StorageProvider { get; set; }

    private void RegisterSaveFileDialog() =>
        ViewModel.SaveFileDialog.RegisterHandler(
            async interaction =>
            {
                List<FileDialogFilter> filters = new();

                List<string> extensions = new();
                extensions.Add("mcp");

                filters.Add(new FileDialogFilter { Name = "MultiConverter Preset file", Extensions = extensions });

                SaveFileDialog dialog = new()
                {
                    Title = "", DefaultExtension = "mcp", InitialFileName = "Preset", Filters = filters
                };

                string? path = await dialog.ShowAsync(App.MainWindow);

                interaction.SetOutput(path);
            });

    private void RegisterOpenFileDialog() =>
        ViewModel.OpenFileDialog.RegisterHandler(
            async interaction =>
            {
                List<FileDialogFilter> filters = new();

                List<string> extensions = new();
                extensions.Add("mcp");

                filters.Add(new FileDialogFilter { Name = "MultiConverter Preset file", Extensions = extensions });

                OpenFileDialog dialog = new()
                {
                    Title = "", InitialFileName = "Preset", Filters = filters, AllowMultiple = false
                };

                string[]? path = await dialog.ShowAsync(App.MainWindow);

                if (path is not null && path.Length > 0)
                {
                    interaction.SetOutput(path[0]);
                }
                else
                {
                    interaction.SetOutput(null);
                }
            });
}
