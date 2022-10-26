using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Moq;
using Moq.AutoMock;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Options.Interfaces;

namespace MultiConverterFixtures.Options;

public abstract class OptionsTestBase
{
    public static AutoMocker GetAutoMocker(ISchedulerProvider? schedulerProvider = null)
    {
        schedulerProvider ??= new ImmediateSchedulers();

        AutoMocker mocker = new();

        mocker.Use(schedulerProvider);

        return mocker;
    }

    public static void SetupGeneralOptions(AutoMocker mocker, GeneralOptions? generalOptions = null)
    {
        Mock<ISetting<GeneralOptions>> setting = mocker.GetMock<ISetting<GeneralOptions>>();
        ReplaySubject<GeneralOptions> generalOptionsSubject = new(1);

        setting.Setup(x => x.Write(It.IsAny<GeneralOptions>()))
            .Callback<GeneralOptions>(item => generalOptionsSubject.OnNext(item));

        generalOptionsSubject.OnNext(generalOptions ?? GeneralOptions.Default());

        setting.SetupGet(x => x.Value).Returns(generalOptionsSubject.AsObservable);

        mocker.Use(setting);
    }

    public static void SetupOptionItems(AutoMocker mocker, IEnumerable<IOptionItem>? optionItems = null)
    {
        if (optionItems == null)
        {
            List<IOptionItem> items = new() { new FakeOptionItem() };
            mocker.Use<IEnumerable<IOptionItem>>(items);
        }
        else
        {
            mocker.Use(optionItems);
        }
    }
}
