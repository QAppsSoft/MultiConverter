﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Binding;
using MultiConverter.Common;
using MultiConverter.Extensions;
using MultiConverter.Models.Presets;
using MultiConverter.Models.PresetsProvider;
using MultiConverter.Services.Abstractions.Dialogs;
using MultiConverter.Services.Abstractions.Presets;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Extensions;
using MultiConverter.ViewModels.Presets.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets;

public sealed class PresetsContainerViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly SourceList<Preset> _presetsSourceList = new();
    private readonly ISchedulerProvider _schedulerProvider;
    private readonly ISetting<Preset[]> _presetsSetting;

    public PresetsContainerViewModel(ISetting<Preset[]> presetsSetting, ISchedulerProvider schedulerProvider,
        IPresetViewModelFactory presetViewModelFactory, IDefaultPresetsProvider presetsProvider,
        IExternalPresetsManager externalPresetsManager, IDialogService dialogService)
    {
        ArgumentNullException.ThrowIfNull(presetsSetting);
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(presetViewModelFactory);
        ArgumentNullException.ThrowIfNull(presetsProvider);
        ArgumentNullException.ThrowIfNull(externalPresetsManager);

        _presetsSetting = presetsSetting;
        _schedulerProvider = schedulerProvider;
        DefaultPresets = presetsProvider.DefaultPresets;

        this.WhenActivated(disposable =>
        {
            HandleActivation();

            Disposable.Create(HandleDeactivation).DisposeWith(disposable);

            var presetsObservable = _presetsSourceList.Connect()
                .Transform(presetViewModelFactory.Build)
                .DisposeMany()
                .Publish();

            InitializePresetsCollection(presetsObservable, disposable);
            UpdateSelectionInPresetsCollectionChanged(disposable);
            OnDefaultSetSwitchOthersToFalse(presetsObservable, disposable);
            InitializeSaveCommand(presetsObservable);
            InitializeResetCommand();
            InitializeAddCommand(presetsObservable);
            InitializeAddPresetCommand(presetsObservable);
            InitializeRemoveCommand();
            InitializeCloneCommand(presetsObservable);
            InitializeImportCommand(dialogService, externalPresetsManager);
            InitializeExportSelectedCommand(dialogService, externalPresetsManager);
            InitializeExportAllCommand(dialogService, externalPresetsManager);

            presetsObservable.Connect().DisposeWith(disposable);

            _presetsSetting.Value
                .Subscribe(SetAndSelectPreset())
                .DisposeWith(disposable);
        });
    }

    public IEnumerable<PresetsProviderItem> DefaultPresets { get; }

    [Reactive] public ReadOnlyObservableCollection<PresetViewModel>? PresetsCollection { get; set; }

    [Reactive] public ReactiveCommand<ReadOnlyObservableCollection<PresetViewModel>, Unit>? Save { get; set; }

    [Reactive] public PresetViewModel? SelectedPreset { get; set; }

    [Reactive] public ReactiveCommand<Unit, Unit>? Reset { get; set; }

    [Reactive] public ReactiveCommand<Unit, Unit>? Add { get; set; }

    [Reactive] public ReactiveCommand<PresetsProviderItem, Unit>? AddDefaultPreset { get; set; }

    [Reactive] public ReactiveCommand<PresetViewModel, Unit>? Remove { get; set; }

    [Reactive] public ReactiveCommand<PresetViewModel, Unit>? Clone { get; set; }

    [Reactive] public ReactiveCommand<Unit, Unit>? Import { get; set; }

    [Reactive] public ReactiveCommand<PresetViewModel, Unit>? ExportSelected { get; set; }

    [Reactive] public ReactiveCommand<IEnumerable<PresetViewModel>, Unit>? ExportAll { get; set; }

    public ViewModelActivator Activator { get; } = new();

    private static async Task SavePresetsFileAsync(Preset[] presets, IDialogService dialogService, IExternalPresetsManager presetsManager)
    {
        FileDialogExtensions[] extensions = { new("MultiConverter Presets Container", new[] { "*.mcp" }) };
        SaveFileDialogSettings options = new(extensions, false, "New Preset", "Select Preset to import");

        string? filePath = await dialogService.ShowSaveFileDialogSelectorAsync(options);

        if (filePath is not null && !await presetsManager.TryExportPresetAsync(presets, filePath))
        {
            // TODO: Signal a failed preset export
        }
    }

    private async Task ImportPresetsAsync(IDialogService dialogService, IExternalPresetsManager presetsManager)
    {
        FileDialogExtensions[] extensions = { new("MultiConverter Presets Container", new[] { "*.mcp" }) };
        OpenFileDialogSettings options = new(false, extensions, "Select Preset to import");

        string[] folderPaths = await dialogService.ShowOpenFileDialogSelectorAsync(options);

        if (folderPaths.Length > 0)
        {
            var presets = await presetsManager.GetPresetAsync(folderPaths[0]);
            _presetsSourceList.AddRange(presets);
        }
    }

    private void InitializeExportSelectedCommand(IDialogService dialogService, IExternalPresetsManager presetsManager)
    {
        ExportSelected = ReactiveCommand.CreateFromTask<PresetViewModel>(async (vm, _) =>
        {
            Preset[] presets = { vm.ToPreset() };
            await SavePresetsFileAsync(presets, dialogService, presetsManager);
        });
    }

    private void InitializeExportAllCommand(IDialogService dialogService, IExternalPresetsManager presetsManager)
    {
        ExportAll = ReactiveCommand.CreateFromTask<IEnumerable<PresetViewModel>>(async (vms, _) =>
        {
            Preset[] presets = vms.Select(vm => vm.ToPreset()).ToArray();
            await SavePresetsFileAsync(presets, dialogService, presetsManager);
        });
    }

    private void InitializeImportCommand(IDialogService dialogService, IExternalPresetsManager presetsManager)
    {
        Import = ReactiveCommand.CreateFromTask(() => ImportPresetsAsync(dialogService, presetsManager));
    }

    private void InitializeAddPresetCommand(IConnectableObservable<IChangeSet<PresetViewModel>> presetsObservable)
    {
        var canAdd = presetsObservable
            .AutoRefresh(vm => vm.Name)
            .Filter(x => x.Name == Preset.Empty.Name)
            .Count()
            .EqualTo(0)
            .StartWith(true);

        AddDefaultPreset = ReactiveCommand.Create<PresetsProviderItem>(item => _presetsSourceList.Add(item.Preset), canAdd);
    }

    private void InitializePresetsCollection(IConnectableObservable<IChangeSet<PresetViewModel>> presetsObservable,
        CompositeDisposable disposable)
    {
        presetsObservable
            .Sort(SortExpressionComparer<PresetViewModel>.Descending(x => x.IsDefault).ThenByAscending(x => x.Name))
            .ObserveOn(_schedulerProvider.Dispatcher)
            .Bind(out ReadOnlyObservableCollection<PresetViewModel>? presetsCollection)
            .Subscribe()
            .DisposeWith(disposable);

        PresetsCollection = presetsCollection;
    }

    private void OnDefaultSetSwitchOthersToFalse(IConnectableObservable<IChangeSet<PresetViewModel>> presetsObservable,
        CompositeDisposable disposable) =>
        presetsObservable.WhenPropertyChanged(vm => vm.IsDefault)
            .Select(propertyValue => propertyValue.Sender)
            .Where(vm => vm.IsDefault)
            .ObserveOn(_schedulerProvider.Dispatcher)
            .Subscribe(defaultPresetViewModel => PresetsCollection?.Where(vm => vm.IsDefault)
                .Where(vm => vm != defaultPresetViewModel)
                .ForEach(vm => vm.IsDefault = false))
            .DisposeWith(disposable);

    private void InitializeRemoveCommand()
    {
        var canRemove = this.WhenAnyValue(vm => vm.SelectedPreset)
            .IsNotNull();

        Remove = ReactiveCommand.Create<PresetViewModel>(
            vm => _presetsSourceList.Remove(vm.InitialPreset),
            canRemove);
    }

    private void InitializeAddCommand(IConnectableObservable<IChangeSet<PresetViewModel>> presetsObservable)
    {
        var canAdd = presetsObservable
            .AutoRefresh(vm => vm.Name)
            .Filter(x => x.Name == Preset.Empty.Name)
            .Count()
            .EqualTo(0)
            .StartWith(true);

        Add = ReactiveCommand.Create(() => _presetsSourceList.Add(Preset.Empty), canAdd);
    }

    private void InitializeResetCommand() =>
        Reset = ReactiveCommand.Create(() =>
        {
            _presetsSourceList.Clear();
            _presetsSetting.Write(Array.Empty<Preset>());
        });

    private void InitializeCloneCommand(IConnectableObservable<IChangeSet<PresetViewModel>> presetsObservable)
    {
        var anySelected = this.WhenAnyValue(vm => vm.SelectedPreset)
            .IsNotNull();

        var selectedNotChanged = this.WhenAnyValue(vm => vm.SelectedPreset)
            .Select(preset => preset is null ? Observable.Return(false) : preset.WhenAnyValue(vm => vm.HasChanged))
            .Switch()
            .InvertValue();

        var noEmptyName = presetsObservable
            .AutoRefresh(vm => vm.Name)
            .Filter(x => x.Name == Preset.Empty.Name)
            .Count()
            .EqualTo(0)
            .StartWith(true);

        var canClone = anySelected.CombineLatest(selectedNotChanged, noEmptyName)
            .Select(tuple => tuple is { First: true, Second: true, Third: true });

        Clone = ReactiveCommand.Create<PresetViewModel>(ClonePreset, canClone);

        void ClonePreset(PresetViewModel preset)
        {
            Preset clonedPreset = (Preset)preset with { Name = string.Empty, IsDefault = false };
            _presetsSourceList.Add(clonedPreset);
        }
    }

    private void InitializeSaveCommand(IConnectableObservable<IChangeSet<PresetViewModel>> presetsObservable)
    {
        var hasChanged = presetsObservable
            .AutoRefresh(vm => vm.HasChanged)
            .Filter(x => x.HasChanged)
            .Count()
            .GreaterThan(0)
            .StartWith(false);

        var notEquals = presetsObservable
            .ToCollection()
            .Select(x => x.Select(y => y.ToPreset()).ToArray())
            .CombineLatest(_presetsSetting.Value,
                (localPresets, settingsPresets) => !localPresets.SequenceEqual(settingsPresets))
            .StartWith(false);

        var noEmptyName = presetsObservable
            .AutoRefresh(vm => vm.Name)
            .Filter(x => x.Name == Preset.Empty.Name)
            .Count()
            .EqualTo(0)
            .StartWith(true);

        var canSave = hasChanged.CombineLatest(notEquals, noEmptyName,
            (changed, notEquals, noEmpty) => noEmpty && (changed || notEquals));

        Save = ReactiveCommand.Create<ReadOnlyObservableCollection<PresetViewModel>>(
            models => _presetsSetting.Write(CastPresets(models)),
            canSave);
    }

    private void UpdateSelectionInPresetsCollectionChanged(CompositeDisposable disposable) =>
        PresetsCollection?.ObserveCollectionChangesOptional<PresetViewModel>()
            .Select(changes => changes.NewItems.HasValue
                ? changes.NewItems.Value.First()
                : PresetsCollection.FirstOrDefault())
            .WhereNotNull()
            .ObserveOn(_schedulerProvider.Dispatcher)
            .Subscribe(preset => SelectedPreset = preset)
            .DisposeWith(disposable);

    private Action<Preset[]> SetAndSelectPreset() =>
        values =>
        {
            _presetsSourceList.Edit(ClearAndAdd(values));

            if (PresetsCollection?.Count > 0)
            {
                SelectedPreset = PresetsCollection.First();
            }
        };

    private static Preset[] CastPresets(IEnumerable<PresetViewModel> presets) =>
        presets.Select(x=> x.ToPreset()).ToArray();

    private static Action<IExtendedList<Preset>> ClearAndAdd(Preset[] values) =>
        cache =>
        {
            cache.Clear();
            cache.AddRange(values);
        };

    private static void HandleActivation()
    {
        // Empty
    }

    private void HandleDeactivation()
    {
        _presetsSourceList.Clear();

        PresetsCollection = null;
        SelectedPreset = null;

        Save?.Dispose();
        Save = null;

        Reset?.Dispose();
        Reset = null;

        Add?.Dispose();
        Add = null;

        AddDefaultPreset?.Dispose();
        AddDefaultPreset = null;

        Remove?.Dispose();
        Remove = null;

        Clone?.Dispose();
        Clone = null;

        Import?.Dispose();
        Import = null;

        ExportSelected?.Dispose();
        ExportSelected = null;

        ExportAll?.Dispose();
        ExportAll = null;
    }
}
