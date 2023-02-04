using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MultiConverter.Common;
using MultiConverter.Localization;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Settings.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Settings;

public sealed class TemporalPathSettingItem : ViewModelBase, ISettingItem, IDisposable
{
    private readonly IDisposable _cleanup;

    public TemporalPathSettingItem(ISchedulerProvider schedulerProvider, ISetting<GeneralOptions> setting,
        IDialogService dialogService)
    {
        IDisposable updateSavedTemporalPath = setting.Value
            .Select(x => x.TemporalFolder)
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(path => TemporalPath = path);

        IDisposable updateCheckTemporalPath = setting.Value
            .Select(x => x.CheckTemporalFolder)
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(shouldCheck => CheckTemporalPath = shouldCheck);

        IDisposable updateCheckEvery = setting.Value
            .Select(x => x.CheckTemporalFolderEvery)
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(every => CheckTemporalPathEvery = every);

        IObservable<string> newTemporalPath = this.WhenAnyValue(x => x.TemporalPath);
        IObservable<bool> newCheckTemporalPath = this.WhenAnyValue(x => x.CheckTemporalPath);
        IObservable<int> newCheckEvery = this.WhenAnyValue(x => x.CheckTemporalPathEvery);

        IObservable<bool> hasChangedPath = setting.Value
            .Select(x => x.TemporalFolder)
            .CombineLatest(newTemporalPath, (savedPath, newPath) => savedPath != newPath);

        IObservable<bool> hasChangedCheck = setting.Value
            .Select(x => x.CheckTemporalFolder)
            .CombineLatest(newCheckTemporalPath, (savedCheck, newCheck) => savedCheck != newCheck);

        IObservable<bool> hasChangedEvery = setting.Value
            .Select(x => x.CheckTemporalFolderEvery)
            .CombineLatest(newCheckEvery, (savedEvery, newEvery) => savedEvery != newEvery);

        var hasChanged = Observable.CombineLatest(hasChangedPath, hasChangedCheck, hasChangedEvery)
            .Select(changed => changed.Any(x => x));

        hasChanged.ToPropertyEx(this, vm => vm.HasChanged);

        UpdateOption = option => option with
        {
            TemporalFolder = TemporalPath,
            CheckTemporalFolder = CheckTemporalPath,
            CheckTemporalFolderEvery = CheckTemporalPathEvery
        };

        ChangeTemporalPath = ReactiveCommand.CreateFromTask(() => UpdateTemporalFolderPath(dialogService));

        _cleanup = new CompositeDisposable(updateSavedTemporalPath, updateCheckTemporalPath, updateCheckEvery);
    }

    private async Task UpdateTemporalFolderPath(IDialogService dialogService)
    {
        const string localizedTitle = "UI_OptionsView_TemporalPathDialogTitle";

        FolderDialogSettings dialogSetting = new(TranslationSource.Instance[localizedTitle], TemporalPath);

        string? newTemporalPath = await dialogService.ShowFolderSelectorAsync(dialogSetting);

        if (newTemporalPath == null)
        {
            return;
        }

        TemporalPath = newTemporalPath;
    }

    [Reactive] public string TemporalPath { get; set; } = string.Empty;

    [Reactive] public bool CheckTemporalPath { get; set; }

    [Reactive] public int CheckTemporalPathEvery { get; set; }

    public ReactiveCommand<Unit, Unit> ChangeTemporalPath { get; }

    [ObservableAsProperty] public bool HasChanged { get; }
    public Func<GeneralOptions, GeneralOptions> UpdateOption { get; }

    public void Dispose()
    {
        _cleanup.Dispose();
        ChangeTemporalPath.Dispose();
    }
}
