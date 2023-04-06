using System;
using MultiConverter.Common;
using MultiConverter.Models.Presets;
using MultiConverter.ViewModels.Presets.Interfaces;

namespace MultiConverter.ViewModels.Presets.Factories;

public sealed class PresetViewModelFactory : IPresetViewModelFactory
{
    private readonly ISchedulerProvider _schedulerProvider;
    private readonly IOptionsViewModelFactory _optionsViewModelFactory;

    public PresetViewModelFactory(ISchedulerProvider schedulerProvider, IOptionsViewModelFactory optionsViewModelFactory)
    {
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(optionsViewModelFactory);

        _schedulerProvider = schedulerProvider;
        _optionsViewModelFactory = optionsViewModelFactory;
    }

    public PresetViewModel Build(Preset preset) => new(preset, _schedulerProvider, _optionsViewModelFactory);
}
