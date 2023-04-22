using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using MultiConverter.Common;
using MultiConverter.Extensions;
using MultiConverter.Models.Presets;
using MultiConverter.ViewModels.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets;

public sealed class PresetViewModel : ViewModelBase, IChanged, IActivatableViewModel, IDisposable
{
    private readonly SourceList<VideoFilter> _videoFilterSourceList = new();
    private readonly SourceList<AudioFilter> _audioFilterSourceList = new();

    public PresetViewModel(Preset preset, ISchedulerProvider schedulerProvider, IOptionsViewModelFactory optionsViewModelFactory)
    {
        ArgumentNullException.ThrowIfNull(preset);
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(optionsViewModelFactory);

        InitialPreset = preset;

        Name = InitialPreset.Name;
        IsDefault = InitialPreset.IsDefault;

        OptionsVm = optionsViewModelFactory.Build(InitialPreset.Options);

        this.WhenActivated(disposable =>
        {
            HandleActivation();

            Disposable.Create(HandleDeactivation).DisposeWith(disposable);

            var hasChanged = HasChangedObservable();

            hasChanged.ToPropertyEx(this, vm => vm.HasChanged).DisposeWith(disposable);
        });
    }

    [ObservableAsProperty] public bool HasChanged { get; }

    [Reactive] public string Name { get; set; }

    [Reactive] public bool IsDefault { get; set; }

    public OptionsViewModel OptionsVm { get; }

    public ViewModelActivator Activator { get; } = new();

    public Preset InitialPreset { get; }

    private static void HandleActivation()
    {
        // Empty
    }

    private static void HandleDeactivation()
    {
        // Empty
    }

    private IObservable<bool> HasChangedObservable()
    {
        var nameHasChanged = this.WhenAnyValue(vm => vm.Name)
            .Select(name => !string.Equals(name, InitialPreset.Name, StringComparison.InvariantCulture));

        var isDefaultHasChanged = this.WhenAnyValue(vm => vm.IsDefault)
            .Select(isDefault => isDefault != InitialPreset.IsDefault);

        var optionsVmHasChanged = OptionsVm.WhenAnyValue(vm => vm.HasChanged);

        var hasChangedObservable = new[] { nameHasChanged, isDefaultHasChanged, optionsVmHasChanged }
            .CombineLatest(statuses => statuses.AnyIsTrue());

        return hasChangedObservable;
    }

    public void Dispose() => OptionsVm.Dispose();

    public Preset ToPreset() => new(
        Name,
        IsDefault,
        Array.Empty<VideoFilter>(), // TODO: Implement VideoFilter
        Array.Empty<AudioFilter>(), // TODO: Implement AudioFilter
        OptionsVm
    );

    public static implicit operator Preset(PresetViewModel vm) => vm.ToPreset();
}
