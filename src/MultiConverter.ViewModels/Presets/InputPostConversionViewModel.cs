using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MultiConverter.Common;
using MultiConverter.Extensions;
using MultiConverter.Models.Presets;
using MultiConverter.Models.Presets.Enums;
using MultiConverter.Services.Abstractions.Dialogs;
using MultiConverter.ViewModels.Presets.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets;

public sealed class InputPostConversionViewModel : ViewModelBase, IChanged, IDisposable
{
    private readonly CompositeDisposable _cleanup = new();

    public InputPostConversionViewModel(InputPostConversion postConversion, ISchedulerProvider schedulerProvider, IDialogService dialogService)
    {
        ArgumentNullException.ThrowIfNull(postConversion);
        ArgumentNullException.ThrowIfNull(schedulerProvider);

        PostConversionAction = postConversion.PostConversionAction;
        ArchiveFolderPath = postConversion.ArchiveFolderPath;
        IncludeProcessingDate = postConversion.IncludeProcessingDate;
        KeepAbsolutePath = postConversion.KeepAbsolutePath;

        this.WhenAnyValue(vm => vm.PostConversionAction)
            .Select(x => x == InputPostConversionAction.MoveToArchiveFolder)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, vm => vm.IsArchiveSelected)
            .DisposeWith(_cleanup);

        HasChangedObservable(postConversion)
            .ToPropertyEx(this, vm => vm.HasChanged)
            .DisposeWith(_cleanup);

        ChangeArchivePath = ReactiveCommand.CreateFromTask(() => UpdateArchivePath(dialogService));
    }

    public InputPostConversionAction[] PostActions { get; } =
    {
        InputPostConversionAction.None,
        InputPostConversionAction.Delete,
        InputPostConversionAction.MoveToArchiveFolder
    };

    [Reactive] public InputPostConversionAction PostConversionAction { get; set; }

    [Reactive] public string ArchiveFolderPath { get; set; }

    [Reactive] public bool IncludeProcessingDate { get; set; }

    [Reactive] public bool KeepAbsolutePath { get; set; }

    public ReactiveCommand<Unit, Unit> ChangeArchivePath { get; }

    [ObservableAsProperty]
    public bool IsArchiveSelected { get; }

    [ObservableAsProperty]
    public bool HasChanged { get; }

    private async Task UpdateArchivePath(IDialogService dialogService)
    {
        // TODO: Localize dialog title
        FolderDialogSettings dialogSetting = string.IsNullOrEmpty(ArchiveFolderPath)
            ? new FolderDialogSettings(false, "Select Archive destination Path")
            : new FolderDialogSettings(false, "Select Archive destination Path", Directory: ArchiveFolderPath);

        string[] archivePath = await dialogService.ShowFolderSelectorAsync(dialogSetting);

        if (archivePath.Length == 0)
        {
            return;
        }

        ArchiveFolderPath = archivePath[0];
    }

    private IObservable<bool> HasChangedObservable(InputPostConversion inputPostConversion)
    {
        var postConversionHasChanged = this.WhenAnyValue(vm => vm.PostConversionAction)
            .Select(x => x != inputPostConversion.PostConversionAction);

        var archiveFolderPathHasChanged = this.WhenAnyValue(vm => vm.ArchiveFolderPath)
            .Select(x => x != inputPostConversion.ArchiveFolderPath);

        var includeProcessingDateHasChanged = this.WhenAnyValue(vm => vm.IncludeProcessingDate)
            .Select(x => x != inputPostConversion.IncludeProcessingDate);

        var keepAbsolutePathHasChanged = this.WhenAnyValue(vm => vm.KeepAbsolutePath)
            .Select(x => x != inputPostConversion.KeepAbsolutePath);

        var hasChangedObservable = new[]
        {
            postConversionHasChanged, archiveFolderPathHasChanged, includeProcessingDateHasChanged,
            keepAbsolutePathHasChanged
        }.CombineLatest(statuses => statuses.AnyIsTrue());

        return hasChangedObservable;
    }

    public static implicit operator InputPostConversion(InputPostConversionViewModel vm) => new(
        vm.PostConversionAction,
        vm.ArchiveFolderPath,
        vm.IncludeProcessingDate,
        vm.KeepAbsolutePath
    );

    public void Dispose() => _cleanup.Dispose();
}
