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

public sealed class PresetViewModel : ViewModelBase, IChanged, IDisposable
{
    private readonly SourceList<VideoFilter> _videoFilterSourceList = new();
    private readonly SourceList<AudioFilter> _audioFilterSourceList = new();

    private readonly CompositeDisposable _cleanup = new();

    public PresetViewModel(Preset preset, ISchedulerProvider schedulerProvider, IOptionsViewModelFactory optionsViewModelFactory, IContainerFormatViewModelFactory containerFormatViewModelFactory)
    {
        ArgumentNullException.ThrowIfNull(preset);
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(optionsViewModelFactory);
        ArgumentNullException.ThrowIfNull(containerFormatViewModelFactory);

        InitialPreset = preset;

        Name = InitialPreset.Name;
        IsDefault = InitialPreset.IsDefault;
        IsAdvanced = InitialPreset.IsAdvanced;

        OptionsVm = optionsViewModelFactory.Build(InitialPreset.Options);
        FormatVm = containerFormatViewModelFactory.Build(preset.ContainerFormat);

        var hasChanged = HasChangedObservable();

        hasChanged.ToPropertyEx(this, vm => vm.HasChanged).DisposeWith(_cleanup);
    }

    [ObservableAsProperty] public bool HasChanged { get; }

    [Reactive] public string Name { get; set; }

    [Reactive] public bool IsDefault { get; set; }

    [Reactive] public bool IsAdvanced { get; set; }

    public OptionsViewModel OptionsVm { get; }

    public ContainerFormatViewModel FormatVm { get; }

    public Preset InitialPreset { get; }

    private IObservable<bool> HasChangedObservable()
    {
        var nameHasChanged = this.WhenAnyValue(vm => vm.Name)
            .Select(name => !string.Equals(name, InitialPreset.Name, StringComparison.InvariantCulture));

        var isDefaultHasChanged = this.WhenAnyValue(vm => vm.IsDefault)
            .Select(isDefault => isDefault != InitialPreset.IsDefault);

        var isAdvancedHasChanged = this.WhenAnyValue(vm => vm.IsAdvanced)
            .Select(isAdvanced => isAdvanced != InitialPreset.IsAdvanced);

        var optionsVmHasChanged = OptionsVm.WhenAnyValue(vm => vm.HasChanged);

        var formatHasChanged = FormatVm.WhenAnyValue(vm => vm.HasChanged);

        var hasChangedObservable =
            new[] { nameHasChanged, isDefaultHasChanged, isAdvancedHasChanged, optionsVmHasChanged, formatHasChanged }
                .CombineLatest(statuses => statuses.AnyIsTrue());

        return hasChangedObservable;
    }

    public void Dispose()
    {
        _cleanup.Dispose();
        OptionsVm.Dispose();
        FormatVm.Dispose();
    }

    public Preset ToPreset() => new(
        Name,
        IsDefault,
        Array.Empty<VideoFilter>(), // TODO: Implement VideoFilter
        Array.Empty<AudioFilter>(), // TODO: Implement AudioFilter
        OptionsVm,
        IsAdvanced,
        FormatVm
    );

    public static implicit operator Preset(PresetViewModel vm) => vm.ToPreset();
}
