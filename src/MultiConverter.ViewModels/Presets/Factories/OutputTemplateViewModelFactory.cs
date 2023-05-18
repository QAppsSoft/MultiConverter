using System;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Output;
using MultiConverter.Services;
using MultiConverter.ViewModels.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Output;

namespace MultiConverter.ViewModels.Presets.Factories;

public sealed class OutputTemplateViewModelFactory : IOutputTemplateViewModelFactory
{
    private readonly ISchedulerProvider _schedulerProvider;
    private readonly IDialogService _dialogService;

    public OutputTemplateViewModelFactory(ISchedulerProvider schedulerProvider, IDialogService dialogService)
    {
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(dialogService);
        _schedulerProvider = schedulerProvider;
        _dialogService = dialogService;
    }

    public OutputTemplateViewModel Build(OutputPathTemplate outputPathTemplate) =>
        new(outputPathTemplate, _schedulerProvider, _dialogService);
}
