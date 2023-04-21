using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Aggregation;
using MultiConverter.Common;
using MultiConverter.Extensions;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Options.Providers;
using MultiConverter.ViewModels.Presets.Options.Providers.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Options;

public sealed class OptionsViewModel : ViewModelBase, IChanged, IDisposable
{
    private readonly IPresetOptionsProvider _presetOptionsProvider;
    private readonly SourceList<IOption> _optionSourceList = new();
    private readonly CompositeDisposable _cleanup = new();

    public OptionsViewModel(IOption[] options, ISchedulerProvider schedulerProvider,
        IOptionsViewModelFactory optionsViewModelFactory, IPresetOptionsProvider presetOptionsProvider)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(optionsViewModelFactory);
        ArgumentNullException.ThrowIfNull(presetOptionsProvider);

        _presetOptionsProvider = presetOptionsProvider;

        _optionSourceList.Edit(cache => cache.AddRange(options));

        var observableOptionViewModel = _optionSourceList.Connect()
            .Transform(optionsViewModelFactory.Build)
            .AutoRefresh(vm => vm.HasChanged)
            .Publish();

        observableOptionViewModel.Filter(vm => vm.HasChanged)
            .Count()
            .GreaterThan(0)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, vm => vm.HasChanged)
            .DisposeWith(_cleanup);

        observableOptionViewModel.ObserveOn(schedulerProvider.Dispatcher)
            .Bind(out var optionsBase)
            .Subscribe()
            .DisposeWith(_cleanup);

        Options = optionsBase;

        observableOptionViewModel.Connect().DisposeWith(_cleanup);
    }

    public ReadOnlyObservableCollection<OptionViewModelBase> Options { get; }

    public IEnumerable<OptionGeneratorBase> NewOptionsList => _presetOptionsProvider.Options;

    [Reactive] public ReactiveCommand<ReadOnlyObservableCollection<OptionViewModelBase>, Unit>? Add { get; set; }

    public void Dispose()
    {
        _cleanup.Dispose();
        _optionSourceList.Dispose();
    }

    [ObservableAsProperty] public bool HasChanged { get; }

    public static implicit operator IOption[](OptionsViewModel vm) => vm.ToOptions();

    public IOption[] ToOptions() => Options.AsEnumerable().Select(x => x.GetOption()).ToArray();
}
