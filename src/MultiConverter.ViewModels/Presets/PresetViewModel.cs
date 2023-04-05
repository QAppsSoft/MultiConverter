using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using MultiConverter.Common;
using MultiConverter.Extensions;
using MultiConverter.Models.Presets;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets;

public class PresetViewModel : ViewModelBase, IChanged, IActivatableViewModel
{
    private readonly Preset _preset;
    private readonly SourceList<VideoFilter> _videoFilterSourceList = new();
    private readonly SourceList<AudioFilter> _audioFilterSourceList = new();
    private readonly SourceList<IOption> _optionSourceList = new();

    public PresetViewModel(Preset preset, ISchedulerProvider schedulerProvider)
    {
        ArgumentNullException.ThrowIfNull(preset);
        ArgumentNullException.ThrowIfNull(schedulerProvider);

        _preset = preset;

        Name = _preset.Name;
        IsDefault = _preset.IsDefault;

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

    public ViewModelActivator Activator { get; } = new();

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
            .Select(name => !string.Equals(name, _preset.Name, StringComparison.InvariantCulture));

        var isDefaultHasChanged = this.WhenAnyValue(vm => vm.IsDefault)
            .Select(isDefault => isDefault != _preset.IsDefault);

        var hasChangedObservable = new[] { nameHasChanged, isDefaultHasChanged }
            .CombineLatest(statuses => statuses.AnyIsTrue());

        return hasChangedObservable;
    }
}
