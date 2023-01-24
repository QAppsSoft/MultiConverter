using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MultiConverter.Common;
using MultiConverter.Models;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Settings.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Settings;

public sealed class LanguageSettingItem : ViewModelBase, ISettingItem, IDisposable
{
    private readonly IDisposable _cleanup;

    public LanguageSettingItem(ISchedulerProvider schedulerProvider, ISetting<GeneralOptions> setting,
        ILanguageManager languageManager)
    {
        Languages = languageManager.AllLanguages;
        SelectedLanguage = languageManager.DefaultLanguage;

        IDisposable updateSavedLanguage = setting.Value
            .Select(x => x.Language)
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(language => SelectedLanguage = Languages.First(model => model.Code == language));

        IObservable<LanguageModel> newSelectedLanguage = this.WhenAnyValue(x => x.SelectedLanguage);

        IObservable<bool> hasChanged = setting.Value
            .Select(x => x.Language)
            .CombineLatest(newSelectedLanguage,
                (savedLanguage, selectedLanguage) => savedLanguage != selectedLanguage.Code);

        hasChanged.ToPropertyEx(this, vm => vm.HasChanged);

        UpdateOption = option => option with { Language = SelectedLanguage.Code };

        _cleanup = updateSavedLanguage;
    }


    public IEnumerable<LanguageModel> Languages { get; }

    [Reactive] public LanguageModel SelectedLanguage { get; set; }

    public void Dispose() => _cleanup.Dispose();

    [ObservableAsProperty] public bool HasChanged { get; }
    public Func<GeneralOptions, GeneralOptions> UpdateOption { get; }
}
