using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using MultiConverter.Configuration;
using MultiConverter.Extensions;
using MultiConverter.Localization;
using MultiConverter.Models;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions;
using MultiConverter.Services.Abstractions.Settings;

namespace MultiConverter.Services.Implementations;

public class LanguageManager : ILanguageManager
{
    private readonly Lazy<Dictionary<string, LanguageModel>> _availableLanguages;
    private readonly LanguagesConfiguration _configuration;

    public LanguageManager(LanguagesConfiguration configuration, ISetting<GeneralOptions> setting)
    {
        _configuration = configuration;
        _availableLanguages = new Lazy<Dictionary<string, LanguageModel>>(GetAvailableLanguages);

        DefaultLanguage = CreateLanguageModel(CultureInfo.GetCultureInfo("en"));

        _ = setting.Value.Subscribe(x => SetLanguage(x.Language));
    }

    public LanguageModel DefaultLanguage { get; }

    public LanguageModel CurrentLanguage => CreateLanguageModel(Thread.CurrentThread.CurrentUICulture);

    public IEnumerable<LanguageModel> AllLanguages => _availableLanguages.Value.Values;

    public void SetLanguage(string languageCode)
    {
        if (string.IsNullOrEmpty(languageCode)) languageCode = "en";

        TranslationSource.Instance.CurrentCulture = new CultureInfo(languageCode);
    }

    public void SetLanguage(LanguageModel languageModel) => SetLanguage(languageModel.Code);

    private Dictionary<string, LanguageModel> GetAvailableLanguages() =>
        _configuration
            .AvailableLocales
            .Select(locale => CreateLanguageModel(new CultureInfo(locale)))
            .ToDictionary(lm => lm.Code, lm => lm);

    private LanguageModel CreateLanguageModel(CultureInfo cultureInfo) =>
        cultureInfo is null
            ? DefaultLanguage
            : new LanguageModel(cultureInfo.EnglishName, cultureInfo.NativeName.ToTitleCase(),
                cultureInfo.TwoLetterISOLanguageName);
}
