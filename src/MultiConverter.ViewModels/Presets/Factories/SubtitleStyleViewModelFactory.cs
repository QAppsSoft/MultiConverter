using System;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Subtitles;
using MultiConverter.ViewModels.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Subtitles;

namespace MultiConverter.ViewModels.Presets.Factories;

public class SubtitleStyleViewModelFactory : ISubtitleStyleViewModelFactory
{
    private readonly ISchedulerProvider _schedulerProvider;

    public SubtitleStyleViewModelFactory(ISchedulerProvider schedulerProvider)
    {
        ArgumentNullException.ThrowIfNull(schedulerProvider);

        _schedulerProvider = schedulerProvider;
    }

    public SubtitleStyleViewModel Build(SubtitleStyle subtitleStyle) => new(subtitleStyle, _schedulerProvider);
}
