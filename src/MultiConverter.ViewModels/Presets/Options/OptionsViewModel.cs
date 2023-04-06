using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Options.Providers;
using MultiConverter.ViewModels.Presets.Options.Providers.Interfaces;

namespace MultiConverter.ViewModels.Presets.Options;

public sealed class OptionsViewModel : ViewModelBase, IDisposable
{
    private readonly IPresetOptionsProvider _presetOptionsProvider;
    private readonly SourceList<IOption> _optionSourceList = new();
    private readonly CompositeDisposable _cleanup = new();

    public OptionsViewModel(IOption[] options, ISchedulerProvider schedulerProvider,
        IOptionsViewModelFactory optionsViewModelFactory,
        IPresetOptionsProvider presetOptionsProvider)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(optionsViewModelFactory);
        ArgumentNullException.ThrowIfNull(presetOptionsProvider);

        _presetOptionsProvider = presetOptionsProvider;

        _optionSourceList.Edit(cache => cache.AddRange(options));

        _optionSourceList.Connect()
            .Transform(optionsViewModelFactory.Build)
            .ObserveOn(schedulerProvider.Dispatcher)
            .Bind(out var optionsBase)
            .Subscribe()
            .DisposeWith(_cleanup);

        Options = optionsBase;
    }

    public ReadOnlyObservableCollection<OptionViewModelBase> Options { get; }

    public IEnumerable<OptionGeneratorBase> NewOptionsList => _presetOptionsProvider.Options;

    public void Dispose()
    {
        _cleanup.Dispose();
        _optionSourceList.Dispose();
    }
}
