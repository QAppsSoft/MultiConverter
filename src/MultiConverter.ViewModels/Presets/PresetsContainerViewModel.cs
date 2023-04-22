using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Binding;
using MultiConverter.Common;
using MultiConverter.Extensions;
using MultiConverter.Models.Presets;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Presets.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets;

public sealed class PresetsContainerViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly SourceList<Preset> _presetsSourceList = new();

    public PresetsContainerViewModel(ISetting<Preset[]> presetsSetting, ISchedulerProvider schedulerProvider, IPresetViewModelFactory presetViewModelFactory)
    {
        ArgumentNullException.ThrowIfNull(presetsSetting);
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(presetViewModelFactory);

        this.WhenActivated(disposable =>
        {
            HandleActivation();

            Disposable.Create(HandleDeactivation).DisposeWith(disposable);

            presetsSetting.Value
                .Subscribe(values => _presetsSourceList.Edit(ClearAndAdd(values)))
                .DisposeWith(disposable);

            var presetsObservable = _presetsSourceList.Connect()
                .Sort(SortExpressionComparer<Preset>.Ascending(x => x.Name).ThenByAscending(x => x.IsDefault))
                .Transform(presetViewModelFactory.Build)
                .DisposeMany()
                .Publish();

            presetsObservable.ObserveOn(schedulerProvider.Dispatcher)
                .Bind(out ReadOnlyObservableCollection<PresetViewModel>? presetsCollection)
                .Subscribe()
                .DisposeWith(disposable);

            PresetsCollection = presetsCollection;

            if (PresetsCollection.Count > 0)
            {
                SelectedPreset = PresetsCollection.First();
            }

            var canSave = presetsObservable.Filter(x => x.HasChanged)
                .Count()
                .GreaterThan(0);

            Save = ReactiveCommand.Create<ReadOnlyObservableCollection<PresetViewModel>>(
                models => presetsSetting.Write(CastPresets(models)),
                canSave);

            Reset = ReactiveCommand.Create(() => presetsSetting.Write(Array.Empty<Preset>()));

            var canAdd = presetsObservable.Filter(x => x.Name == Preset.Empty.Name)
                .Count()
                .EqualTo(0);

            Add = ReactiveCommand.Create(() => _presetsSourceList.Add(Preset.Empty), canAdd);

            presetsObservable.Connect().DisposeWith(disposable);
        });
    }

    [Reactive] public ReadOnlyObservableCollection<PresetViewModel>? PresetsCollection { get; set; }

    [Reactive] public ReactiveCommand<ReadOnlyObservableCollection<PresetViewModel>, Unit>? Save { get; set; }

    [Reactive] public PresetViewModel? SelectedPreset { get; set; }

    [Reactive] public ReactiveCommand<Unit, Unit>? Reset { get; set; }

    [Reactive] public ReactiveCommand<Unit, Unit>? Add { get; set; }

    public ViewModelActivator Activator { get; } = new();

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
    }
}
