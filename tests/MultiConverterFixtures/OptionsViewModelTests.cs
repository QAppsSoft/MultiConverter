using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
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

namespace MultiConverterFixtures;

public class OptionsViewModelTests
{
    private AutoMocker GetAutoMocker(ISchedulerProvider? schedulerProvider = null)
    {
        schedulerProvider ??= new TestSchedulers();

        AutoMocker mocker = new();

        mocker.Use(schedulerProvider);

        Mock<ILanguageManager> languageManager = mocker.GetMock<ILanguageManager>();
        languageManager.SetupGet(x => x.AllLanguages).Returns(new[]
        {
            new LanguageModel("Spanish", "Español", "es"),
            new LanguageModel("English", "English", "en")
        });

        return mocker;
    }

    [Test]
    public void Check_viewmodel_status_before_activation()
    {
        bool? canExecute = null;

        AutoMocker mocker = GetAutoMocker();

        Mock<ISetting<GeneralOptions>> setting = mocker.GetMock<ISetting<GeneralOptions>>();
        setting.SetupGet(x => x.Value).Returns(Observable.Return(GeneralOptions.Default()));

        OptionsViewModel fixture = mocker.CreateInstance<OptionsViewModel>();


        _ = fixture.Save.CanExecute.Subscribe(value => canExecute = value);


        fixture.SelectedTheme.Should().Be(default);
        fixture.SelectedLanguage.Should().NotBeNull();
        fixture.AnalysisTimeout.Should().Be(0);
        fixture.LoadFilesAlreadyInQueue.Should().Be(false);
        fixture.TemporalPath.Should().BeEmpty();
        fixture.FileFilters.Should().BeEmpty();
        fixture.SupportedExtensions.Should().BeEmpty();
        canExecute.Should().BeFalse();
    }

    [Test]
    public void Check_viewmodel_status_after_activation()
    {
        bool? canExecute = null;
        TestSchedulers schedulerProvider = new();

        AutoMocker mocker = GetAutoMocker(schedulerProvider);

        Mock<ISetting<GeneralOptions>> setting = mocker.GetMock<ISetting<GeneralOptions>>();
        setting.SetupGet(x => x.Value).Returns(Observable.Return(GeneralOptions.Default()));

        OptionsViewModel fixture = mocker.CreateInstance<OptionsViewModel>();


        fixture.Activator.Activate();
        _ = fixture.Save.CanExecute.Subscribe(value => canExecute = value);
        schedulerProvider.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        schedulerProvider.Dispatcher.Start();


        fixture.SelectedTheme.Should().Be(GeneralOptions.Default().Theme);
        fixture.SelectedLanguage.Code.Should().Be(GeneralOptions.Default().Language);
        fixture.AnalysisTimeout.Should().Be(GeneralOptions.Default().AnalysisTimeout);
        fixture.LoadFilesAlreadyInQueue.Should().Be(GeneralOptions.Default().LoadFilesAlreadyInQueue);
        fixture.TemporalPath.Should().Be(GeneralOptions.Default().TemporalFolder);
        fixture.FileFilters.Should().BeEmpty();
        fixture.SupportedExtensions.Should().NotBeEmpty();
        canExecute.Should().BeFalse();
    }

    [Test]
    public void Check_viewmodel_status_after_activation_and_deactivation()
    {
        bool? canExecute = null;
        TestSchedulers schedulerProvider = new();

        AutoMocker mocker = GetAutoMocker(schedulerProvider);

        Mock<ISetting<GeneralOptions>> setting = mocker.GetMock<ISetting<GeneralOptions>>();
        setting.SetupGet(x => x.Value).Returns(Observable.Return(GeneralOptions.Default()));

        OptionsViewModel fixture = mocker.CreateInstance<OptionsViewModel>();


        fixture.Activator.Activate();
        _ = fixture.Save.CanExecute.Subscribe(value => canExecute = value);
        schedulerProvider.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        schedulerProvider.Dispatcher.Start();
        fixture.Activator.Deactivate();


        fixture.SelectedTheme.Should().Be(GeneralOptions.Default().Theme);
        fixture.SelectedLanguage.Code.Should().Be(GeneralOptions.Default().Language);
        fixture.AnalysisTimeout.Should().Be(GeneralOptions.Default().AnalysisTimeout);
        fixture.LoadFilesAlreadyInQueue.Should().Be(GeneralOptions.Default().LoadFilesAlreadyInQueue);
        fixture.TemporalPath.Should().Be(GeneralOptions.Default().TemporalFolder);
        fixture.FileFilters.Should().BeEmpty();
        fixture.SupportedExtensions.Should().NotBeEmpty();
        canExecute.Should().BeFalse();
    }

    [Test]
    public async Task Check_viewmodel_saveButton_after_activation()
    {
        bool? canExecute = null;
        TestSchedulers schedulerProvider = new();

        AutoMocker mocker = GetAutoMocker(schedulerProvider);

        Mock<ISetting<GeneralOptions>> setting = mocker.GetMock<ISetting<GeneralOptions>>();
        setting.SetupGet(x => x.Value).Returns(Observable.Return(GeneralOptions.Default()));

        OptionsViewModel fixture = mocker.CreateInstance<OptionsViewModel>();


        fixture.Activator.Activate();
        _ = fixture.Save.CanExecute.Subscribe(value => canExecute = value);
        schedulerProvider.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        schedulerProvider.Dispatcher.Start();


        fixture.FileFilters.Count.Should().Be(0);
        fixture.SupportedExtensions.Count.Should().Be(GeneralOptions.Default().SupportedFilesExtensions.Length);
        canExecute.Should().BeFalse();
    }

    [Test]
    public async Task Check_viewmodel_execute_saveButton_after_activation_and_property_changed()
    {
        GeneralOptions defaultOptions = GeneralOptions.Default();
        GeneralOptions defaultWithLanguageUpdated = defaultOptions with { Language = "es" };
        GeneralOptions options = GeneralOptions.Default();
        ISubject<GeneralOptions> subject = new ReplaySubject<GeneralOptions>(1);
        TestSchedulers schedulerProvider = new();

        AutoMocker mocker = GetAutoMocker(schedulerProvider);

        subject.OnNext(defaultOptions);
        subject.Subscribe(x => options = x);

        Mock<ISetting<GeneralOptions>> setting = mocker.GetMock<ISetting<GeneralOptions>>();
        setting.SetupGet(x => x.Value).Returns(subject.AsObservable);
        setting.Setup(x => x.Write(It.IsAny<GeneralOptions>())).Callback<GeneralOptions>(x => subject.OnNext(x));

        OptionsViewModel fixture = mocker.CreateInstance<OptionsViewModel>();


        fixture.Activator.Activate();
        schedulerProvider.Dispatcher.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);

        fixture.SelectedLanguage = fixture.Languages.First(x => x.Code == "es");

        schedulerProvider.Dispatcher.Start();

        fixture.Save.Execute().Subscribe(_ => { });


        fixture.SelectedLanguage.Code.Should().Be("es");
        options.Language.Should().Be("es");
        options.Should().NotBe(defaultOptions);
        options.Should().Be(defaultWithLanguageUpdated);
    }

    [Test]
    public void Check_viewmodel_appThemes_output()
    {
        AutoMocker mocker = GetAutoMocker();

        Mock<ISetting<GeneralOptions>> setting = mocker.GetMock<ISetting<GeneralOptions>>();
        setting.SetupGet(x => x.Value).Returns(Observable.Return(GeneralOptions.Default()));

        OptionsViewModel fixture = mocker.CreateInstance<OptionsViewModel>();

        fixture.AppThemes.Should().NotBeEmpty();
        fixture.AppThemes.Count().Should().Be(2);
        fixture.AppThemes.Should().BeEquivalentTo(new[] { Theme.Dark, Theme.Light });
    }
}
