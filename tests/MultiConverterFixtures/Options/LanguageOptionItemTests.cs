﻿using System.Linq;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Options;
using NUnit.Framework;

namespace MultiConverterFixtures.Options;

public class LanguageOptionItemTests
{
    private static AutoMocker GetAutoMocker(ISchedulerProvider? schedulerProvider = null)
    {
        schedulerProvider ??= new ImmediateSchedulers();

        AutoMocker mocker = new();

        mocker.Use(schedulerProvider);

        Mock<ILanguageManager> languageManager = mocker.GetMock<ILanguageManager>();
        languageManager.SetupGet(x => x.AllLanguages).Returns(new[]
        {
            new LanguageModel("Spanish", "Español", "es"), new LanguageModel("English", "English", "en")
        });

        return mocker;
    }

    private static void SetupGeneralOptions(AutoMocker mocker, GeneralOptions? generalOptions = null)
    {
        Mock<ISetting<GeneralOptions>> setting = mocker.GetMock<ISetting<GeneralOptions>>();

        if (generalOptions.HasValue)
        {
            setting.SetupGet(x => x.Value).Returns(Observable.Return(generalOptions.Value));
        }
        else
        {
            setting.SetupGet(x => x.Value).Returns(Observable.Return(GeneralOptions.Default()));
        }
    }

    [Test]
    public void LanguageOptionItem_after_initialization()
    {
        ImmediateSchedulers scheduler = new();
        AutoMocker mocker = GetAutoMocker(scheduler);
        SetupGeneralOptions(mocker);
        using LanguageOptionItem fixture = mocker.CreateInstance<LanguageOptionItem>();

        fixture.Languages.Count().Should().Be(2);
        fixture.Languages.Select(x => x.Code).Should().BeEquivalentTo("en", "es");
        fixture.SelectedLanguage.Code.Should().Be("en");
        fixture.HasChanged.Should().BeFalse();
    }

    [Test]
    public void LanguageOptionItem_when_changed_HasChanged_should_be_true()
    {
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using LanguageOptionItem fixture = mocker.CreateInstance<LanguageOptionItem>();

        fixture.SelectedLanguage = fixture.Languages.First(x => x.Code == "es");

        fixture.SelectedLanguage.Code.Should().Be("es");
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void LanguageOptionItem_when_changed_UpdateOption_should_change_language()
    {
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using LanguageOptionItem fixture = mocker.CreateInstance<LanguageOptionItem>();

        fixture.SelectedLanguage = fixture.Languages.First(x => x.Code == "es");
        GeneralOptions option = fixture.UpdateOption(GeneralOptions.Default());

        fixture.SelectedLanguage.Code.Should().Be("es");
        fixture.HasChanged.Should().BeTrue();
        option.Language.Should().Be("es");
    }
}
