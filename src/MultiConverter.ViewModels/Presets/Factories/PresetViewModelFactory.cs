﻿using System;
using MultiConverter.Common;
using MultiConverter.Models.Presets;
using MultiConverter.ViewModels.Presets.Interfaces;

namespace MultiConverter.ViewModels.Presets.Factories;

public sealed class PresetViewModelFactory : IPresetViewModelFactory
{
    private readonly ISchedulerProvider _schedulerProvider;
    private readonly IOptionsViewModelFactory _optionsViewModelFactory;
    private readonly IContainerFormatViewModelFactory _containerFormatViewModelFactory;
    private readonly IPostConversionViewModelFactory _postConversionViewModelFactory;

    public PresetViewModelFactory(ISchedulerProvider schedulerProvider, IOptionsViewModelFactory optionsViewModelFactory, IContainerFormatViewModelFactory containerFormatViewModelFactory, IPostConversionViewModelFactory postConversionViewModelFactory)
    {
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(optionsViewModelFactory);
        ArgumentNullException.ThrowIfNull(containerFormatViewModelFactory);
        ArgumentNullException.ThrowIfNull(postConversionViewModelFactory);

        _schedulerProvider = schedulerProvider;
        _optionsViewModelFactory = optionsViewModelFactory;
        _containerFormatViewModelFactory = containerFormatViewModelFactory;
        _postConversionViewModelFactory = postConversionViewModelFactory;
    }

    public PresetViewModel Build(Preset preset) =>
        new(
            preset,
            _schedulerProvider,
            _optionsViewModelFactory,
            _containerFormatViewModelFactory,
            _postConversionViewModelFactory);
}
