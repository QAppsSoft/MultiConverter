using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using MultiConverter.Configuration;
using MultiConverter.Extensions;
using MultiConverter.Localization;
using MultiConverter.Models;
using MultiConverter.Services.Implementations;
using NUnit.Framework;

namespace MultiConverterFixtures;

public class LanguageManagerTests
{
    private readonly LanguagesConfiguration _config = new() { AvailableLocales = new List<string> { "en", "es" } };
    private readonly Func<LanguagesConfiguration, LanguageManager> _getManager = config => new LanguageManager(config);

    [Test]
    public void Available_languages_should_be()
    {
        LanguageManager languageManager = _getManager(_config);

        languageManager.AllLanguages.Should().NotBeEmpty();
        languageManager.AllLanguages.Count().Should().Be(2);
        languageManager.AllLanguages.Select(x => x.Code).Should().BeEquivalentTo("es", "en");
    }

    [Test]
    public void Default_language_should_be_english()
    {
        LanguageManager languageManager = _getManager(_config);

        languageManager.DefaultLanguage.Code.Should().Be("en");
    }

    [Test]
    public void SetLanguage()
    {
        LanguageManager languageManager = _getManager(_config);
        var languages = languageManager.AllLanguages.ToArray();

        languageManager.SetLanguage(languages[0]);
        TranslationSource.Instance.CurrentCulture.Should().Be(new CultureInfo(languages[0].Code));

        languageManager.SetLanguage(languages[1]);
        TranslationSource.Instance.CurrentCulture.Should().Be(new CultureInfo(languages[1].Code));
    }

    [Test]
    public void SetLanguage_with_empty_value()
    {
        LanguageManager languageManager = _getManager(_config);

        languageManager.SetLanguage(string.Empty);
        TranslationSource.Instance.CurrentCulture.Should().Be(new CultureInfo("en"));
    }



}
