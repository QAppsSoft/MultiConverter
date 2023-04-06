using System;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using MultiConverter.ViewModels.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Options;
using MultiConverter.ViewModels.Presets.Options.Providers.Interfaces;

namespace MultiConverter.ViewModels.Presets.Factories;

public sealed class OptionsViewModelFactory : IOptionsViewModelFactory
{
    private readonly ISchedulerProvider _schedulerProvider;
    private readonly IPresetOptionsProvider _presetOptionsProvider;

    public OptionsViewModelFactory(ISchedulerProvider schedulerProvider, IPresetOptionsProvider presetOptionsProvider)
    {
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(presetOptionsProvider);

        _schedulerProvider = schedulerProvider;
        _presetOptionsProvider = presetOptionsProvider;
    }

    public OptionsViewModel Build(IOption[] options) => new(options, _schedulerProvider, this, _presetOptionsProvider);

    public OptionViewModelBase Build(IOption option) =>
        option switch
        {
            AudioBitrateOption audioBitrateOption => new AudioBitrateOptionViewModel(audioBitrateOption, _schedulerProvider),
            VideoFrameRateOption videoFrameRateOption => new VideoFrameRateOptionViewModel(videoFrameRateOption, _schedulerProvider),
            _ => throw new ArgumentOutOfRangeException(nameof(option))
        };
}
