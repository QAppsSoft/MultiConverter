using System;
using System.Reactive.Linq;
using Avalonia;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;
using Microsoft.Extensions.Logging;
using MultiConverter.Common;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions;
using MultiConverter.Services.Abstractions.Settings;
using Theme = MultiConverter.Models.Settings.General.Theme;

namespace MultiConverter.Services.Implementations;

public class SystemSetterJob : ISystemSetterJob
{
    private static readonly MaterialTheme s_materialThemeStyles =
        Application.Current!.LocateMaterialTheme<MaterialTheme>();

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

    private static void SetTheme(Theme theme) => s_materialThemeStyles.BaseTheme =
        theme == Theme.Dark ? BaseThemeMode.Dark : BaseThemeMode.Light;

    private void SetLanguage(string language) => _languageManager.SetLanguage(language);
}
