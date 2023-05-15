using System;
using MultiConverter.Common;
using MultiConverter.Models.Presets;
using MultiConverter.Services;
using MultiConverter.ViewModels.Presets.Interfaces;

namespace MultiConverter.ViewModels.Presets.Factories;

public class PostConversionViewModelFactory : IPostConversionViewModelFactory
{
    private readonly ISchedulerProvider _schedulerProvider;
    private readonly IDialogService _dialogService;

    public PostConversionViewModelFactory(ISchedulerProvider schedulerProvider, IDialogService dialogService)
    {
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(dialogService);

        _schedulerProvider = schedulerProvider;
        _dialogService = dialogService;
    }

    public InputPostConversionViewModel Build(InputPostConversion presetPostConversion) =>
        new(presetPostConversion, _schedulerProvider, _dialogService);
}
