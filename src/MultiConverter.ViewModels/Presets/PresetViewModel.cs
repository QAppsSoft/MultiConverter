using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using MultiConverter.Common;
using MultiConverter.Extensions;
using MultiConverter.Models.Presets;
using MultiConverter.ViewModels.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Options;
using MultiConverter.ViewModels.Presets.Subtitles;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets;

public sealed class PresetViewModel : ViewModelBase, IChanged, IDisposable
{
    private readonly SourceList<VideoFilter> _videoFilterSourceList = new();
    private readonly SourceList<AudioFilter> _audioFilterSourceList = new();

    private readonly CompositeDisposable _cleanup = new();

    public PresetViewModel(Preset preset, ISchedulerProvider schedulerProvider,
        IOptionsViewModelFactory optionsViewModelFactory,
        IContainerFormatViewModelFactory containerFormatViewModelFactory,
        IPostConversionViewModelFactory postConversionViewModelFactory,
        ISubtitleStyleViewModelFactory subtitleStyleViewModelFactory)
    {
        ArgumentNullException.ThrowIfNull(preset);
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(optionsViewModelFactory);
        ArgumentNullException.ThrowIfNull(containerFormatViewModelFactory);
        ArgumentNullException.ThrowIfNull(postConversionViewModelFactory);
        ArgumentNullException.ThrowIfNull(subtitleStyleViewModelFactory);

        InitialPreset = preset;

        Name = InitialPreset.Name;
        IsDefault = InitialPreset.IsDefault;
        IsAdvanced = InitialPreset.IsAdvanced;

        OptionsVm = optionsViewModelFactory.Build(preset.Options);
        FormatVm = containerFormatViewModelFactory.Build(preset.ContainerFormat);
        PostConversionVm = postConversionViewModelFactory.Build(preset.PostConversion);
        SubtitleStyleVm = subtitleStyleViewModelFactory.Build(preset.SubtitleStyle);

        var hasChanged = HasChangedObservable();

        hasChanged.ToPropertyEx(this, vm => vm.HasChanged).DisposeWith(_cleanup);
    }

    [ObservableAsProperty] public bool HasChanged { get; }

    [Reactive] public string Name { get; set; }

    [Reactive] public bool IsDefault { get; set; }

    [Reactive] public bool IsAdvanced { get; set; }

    public OptionsViewModel OptionsVm { get; }

    public ContainerFormatViewModel FormatVm { get; }

    public InputPostConversionViewModel PostConversionVm { get; }

    public SubtitleStyleViewModel SubtitleStyleVm { get; }

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

        var postConversionChanged = PostConversionVm.WhenAnyValue(vm => vm.HasChanged);

        var subtitleStyleChanged = SubtitleStyleVm.WhenAnyValue(vm => vm.HasChanged);

        var hasChangedObservable =
            new[]
                {
                    nameHasChanged, isDefaultHasChanged, isAdvancedHasChanged, optionsVmHasChanged,
                    formatHasChanged, postConversionChanged, subtitleStyleChanged
                }
                .CombineLatest(statuses => statuses.AnyIsTrue());

        return hasChangedObservable;
    }

    public void Dispose()
    {
        _cleanup.Dispose();
        OptionsVm.Dispose();
        FormatVm.Dispose();
        PostConversionVm.Dispose();
        SubtitleStyleVm.Dispose();
    }

    public Preset ToPreset() => new(
        Name,
        IsDefault,
        Array.Empty<VideoFilter>(), // TODO: Implement VideoFilter
        Array.Empty<AudioFilter>(), // TODO: Implement AudioFilter
        OptionsVm,
        IsAdvanced,
        FormatVm,
        PostConversionVm,
        SubtitleStyleVm
    );

    public static implicit operator Preset(PresetViewModel vm) => vm.ToPreset();
}
