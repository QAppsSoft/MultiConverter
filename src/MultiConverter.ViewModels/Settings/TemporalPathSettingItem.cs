﻿using System;
using System.Reactive;
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

        IObservable<string> newTemporalPath = this.WhenAnyValue(x => x.TemporalPath);

        var hasChanged = setting.Value
            .Select(x => x.TemporalFolder)
            .CombineLatest(newTemporalPath, (savedPath, newPath) => savedPath != newPath);

        hasChanged.ToPropertyEx(this, vm => vm.HasChanged);

        UpdateOption = option => option with { TemporalFolder = TemporalPath };

        ChangeTemporalPath = ReactiveCommand.CreateFromTask(() => UpdateTemporalFolderPath(dialogService));

        _cleanup = updateSavedTemporalPath;
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

    public ReactiveCommand<Unit, Unit> ChangeTemporalPath { get; }

    [ObservableAsProperty] public bool HasChanged { get; }
    public Func<GeneralOptions, GeneralOptions> UpdateOption { get; }

    public void Dispose()
    {
        _cleanup.Dispose();
        ChangeTemporalPath.Dispose();
    }
}