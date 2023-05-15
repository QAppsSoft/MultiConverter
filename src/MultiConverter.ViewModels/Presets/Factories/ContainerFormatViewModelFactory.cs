using System;
using MultiConverter.Common;
using MultiConverter.Services.Abstractions.Formats;
using MultiConverter.ViewModels.Presets.Interfaces;

namespace MultiConverter.ViewModels.Presets.Factories;

public class ContainerFormatViewModelFactory : IContainerFormatViewModelFactory
{
    private readonly IContainersFormatProvider _formatProvider;
    private readonly ISchedulerProvider _schedulerProvider;

    public ContainerFormatViewModelFactory(IContainersFormatProvider formatProvider, ISchedulerProvider schedulerProvider)
    {
        ArgumentNullException.ThrowIfNull(formatProvider);
        ArgumentNullException.ThrowIfNull(schedulerProvider);

        _formatProvider = formatProvider;
        _schedulerProvider = schedulerProvider;
    }

    public ContainerFormatViewModel Build(string containerFormat) =>
        new(containerFormat, _formatProvider, _schedulerProvider);
}
