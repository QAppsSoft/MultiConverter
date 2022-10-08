using System;
using System.Reactive.Linq;
using FluentAvalonia.Styling;
using Microsoft.Extensions.Logging;
using MultiConverter.Common;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions;
using MultiConverter.Services.Abstractions.Settings;

namespace MultiConverter.Services.Implementations;

public class SystemSetterJob : ISystemSetterJob
{
    private readonly FluentAvaloniaTheme _fluentAvaloniaTheme;
    private readonly ILanguageManager _languageManager;

    public SystemSetterJob(ISchedulerProvider schedulerProvider, ILogger logger, ISetting<GeneralOptions> setting,
        ILanguageManager languageManager, FluentAvaloniaTheme fluentAvaloniaTheme)
    {
        _languageManager = languageManager;
        _fluentAvaloniaTheme = fluentAvaloniaTheme;

        _ = setting.Value
            .Select(options => options.Language)
            .Subscribe(SetLanguage);

        _ = setting.Value
            .Select(options => options.Theme)
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(SetTheme);
    }

    private void SetTheme(Theme theme) =>
        _fluentAvaloniaTheme.RequestedTheme = theme == Theme.Dark
            ? FluentAvaloniaTheme.DarkModeString
            : FluentAvaloniaTheme.LightModeString;

    private void SetLanguage(string language) => _languageManager.SetLanguage(language);
}
