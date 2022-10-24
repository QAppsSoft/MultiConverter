using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.FrameworkDialogs;
using MultiConverter.Common;
using MultiConverter.Localization;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Options.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Options;

public sealed class TemporalPathOptionItem : ViewModelBase, IOptionItem, IDisposable
{
    private readonly IDisposable _cleanup;

    public TemporalPathOptionItem(ISchedulerProvider schedulerProvider, ISetting<GeneralOptions> setting,
        IDialogService dialogService)
    {
        IDisposable updateSavedTemporalPath = setting.Value
            .Select(x => x.TemporalFolder)
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(path => TemporalPath = path);

        IObservable<string> newTemporalPath = this.WhenAnyValue(x => x.TemporalPath);

        HasChanged = setting.Value
            .Select(x => x.TemporalFolder)
            .CombineLatest(newTemporalPath, (savedPath, newPath) => savedPath != newPath);

        UpdateOption = option => option with { TemporalFolder = TemporalPath };

        ChangeTemporalPath = ReactiveCommand.CreateFromTask(() => UpdateTemporalFolderPath(dialogService));

        _cleanup = updateSavedTemporalPath;
    }

    private async Task UpdateTemporalFolderPath(IDialogService dialogService)
    {
        const string localizedTitle = "UI_OptionsView_TemporalPathDialogTitle";

        OpenFolderDialogSettings dialogSetting =
            new()
            {
                InitialDirectory = TemporalPath,
                Title = TranslationSource.Instance[localizedTitle]
            };

        string? newTemporalPath =
            await dialogService.ShowOpenFolderDialogAsync(null, dialogSetting)
                .ConfigureAwait(false);

        if (newTemporalPath == null)
        {
            return;
        }

        TemporalPath = newTemporalPath;
    }

    [Reactive] public string TemporalPath { get; set; } = string.Empty;

    public ReactiveCommand<Unit, Unit> ChangeTemporalPath { get; }

    public IObservable<bool> HasChanged { get; }
    public Func<GeneralOptions, GeneralOptions> UpdateOption { get; }

    public void Dispose()
    {
        _cleanup.Dispose();
        ChangeTemporalPath.Dispose();
    }
}
