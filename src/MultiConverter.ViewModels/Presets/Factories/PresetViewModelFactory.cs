using System;
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
    private readonly ISubtitleStyleViewModelFactory _subtitleStyleViewModelFactory;

    public PresetViewModelFactory(ISchedulerProvider schedulerProvider,
        IOptionsViewModelFactory optionsViewModelFactory,
        IContainerFormatViewModelFactory containerFormatViewModelFactory,
        IPostConversionViewModelFactory postConversionViewModelFactory,
        ISubtitleStyleViewModelFactory subtitleStyleViewModelFactory)
    {
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(optionsViewModelFactory);
        ArgumentNullException.ThrowIfNull(containerFormatViewModelFactory);
        ArgumentNullException.ThrowIfNull(postConversionViewModelFactory);
        ArgumentNullException.ThrowIfNull(subtitleStyleViewModelFactory);

        _schedulerProvider = schedulerProvider;
        _optionsViewModelFactory = optionsViewModelFactory;
        _containerFormatViewModelFactory = containerFormatViewModelFactory;
        _postConversionViewModelFactory = postConversionViewModelFactory;
        _subtitleStyleViewModelFactory = subtitleStyleViewModelFactory;
    }

    public PresetViewModel Build(Preset preset) => new(
        preset,
        _schedulerProvider,
        _optionsViewModelFactory,
        _containerFormatViewModelFactory,
        _postConversionViewModelFactory,
        _subtitleStyleViewModelFactory
    );
}
