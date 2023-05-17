using System;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Output;
using MultiConverter.ViewModels.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Output;

namespace MultiConverter.ViewModels.Presets.Factories;

public sealed class OutputTemplateViewModelFactory : IOutputTemplateViewModelFactory
{
    private readonly ISchedulerProvider _schedulerProvider;

    public OutputTemplateViewModelFactory(ISchedulerProvider schedulerProvider)
    {
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        _schedulerProvider = schedulerProvider;
    }

    public OutputTemplateViewModel Build(OutputPathTemplate outputPathTemplate) =>
        new(outputPathTemplate, _schedulerProvider);
}
