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
            VideoCodecOption videoCodecOption => new VideoCodecOptionViewModel(videoCodecOption, _schedulerProvider),
            VideoBitrateOption videoBitrateOption => new VideoBitrateOptionViewModel(videoBitrateOption, _schedulerProvider),
            VideoAspectRatioOption videoAspectRatioOption => new VideoAspectRatioOptionViewModel(videoAspectRatioOption, _schedulerProvider),
            VideoFrameRateOption videoFrameRateOption => new VideoFrameRateOptionViewModel(videoFrameRateOption, _schedulerProvider),

            AudioCodecOption audioCodecOption => new AudioCodecOptionViewModel(audioCodecOption, _schedulerProvider),
            AudioBitrateOption audioBitrateOption => new AudioBitrateOptionViewModel(audioBitrateOption, _schedulerProvider),
            AudioSamplingRateOption audioSamplingRateOption => new AudioSamplingRateOptionViewModel(audioSamplingRateOption, _schedulerProvider),

            _ => throw new ArgumentOutOfRangeException(nameof(option))
        };
}
