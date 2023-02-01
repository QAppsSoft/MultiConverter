﻿using System;
using System.Reactive.Linq;
using Avalonia;
using FluentAvalonia.Styling;
using Microsoft.Extensions.Logging;
using MultiConverter.Common;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions;
using MultiConverter.Services.Abstractions.Settings;
using Theme = MultiConverter.Models.Settings.General.Theme;

namespace MultiConverter.Services.Implementations;

public class SystemSetterJob : ISystemSetterJob
{
    private readonly ILanguageManager _languageManager;

    public SystemSetterJob(ISchedulerProvider schedulerProvider, ILogger<SystemSetterJob> logger,
        ISetting<GeneralOptions> setting,
        ILanguageManager languageManager)
    {
        _languageManager = languageManager;

        _ = setting.Value
            .Select(options => options.Language)
            .Subscribe(SetLanguage);

        _ = setting.Value
            .Select(options => options.Theme)
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(SetTheme);
    }

    private static void SetTheme(Theme theme)
    {
        FluentAvaloniaTheme? faTheme = AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>();

        if (faTheme is null)
        {
            return;
        }

        if (theme == Theme.Dark)
        {
            faTheme.RequestedTheme = FluentAvaloniaTheme.DarkModeString;
        }
        else
        {
            faTheme.RequestedTheme = FluentAvaloniaTheme.LightModeString;
        }
    }

    private void SetLanguage(string language) => _languageManager.SetLanguage(language);
}
