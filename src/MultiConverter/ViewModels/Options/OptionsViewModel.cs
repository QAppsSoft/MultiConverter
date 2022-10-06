using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using DynamicData.Kernel;
using MultiConverter.Common;
using MultiConverter.Models;
using MultiConverter.Models.Settings.General;
using MultiConverter.Models.Settings.General.FileFilters;
using MultiConverter.Services.Abstractions;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.Utils;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Options;

public class OptionsViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly ISourceList<FileFilter> _fileFilters = new SourceList<FileFilter>();

    private readonly ISourceList<string> _supportedExtensions = new SourceList<string>();

    public OptionsViewModel(ISchedulerProvider schedulerProvider, ISetting<GeneralOptions> setting, ILanguageManager languageManager)
    {
        Activator = new ViewModelActivator();
        this.WhenActivated(disposable =>
        {
            HandleActivation(schedulerProvider, setting, disposable);
            Disposable.Create(HandleDeactivation).DisposeWith(disposable);
        });

        Languages = languageManager.AllLanguages;
        SelectedLanguage = languageManager.AllLanguages.First();

        _ = _fileFilters.Connect()
            .ObserveOn(schedulerProvider.Dispatcher)
            .Bind(out ReadOnlyObservableCollection<FileFilter> filters)
            .Subscribe();

        FileFilters = filters;

        _ = _supportedExtensions.Connect()
            .Sort(SortExpressionComparer<string>.Ascending(x => x))
            .ObserveOn(schedulerProvider.Dispatcher)
            .Bind(out ReadOnlyObservableCollection<string> extensions)
            .Subscribe();

        SupportedExtensions = extensions;

        IObservable<GeneralOptions> newOptions = NewOptionObservable().Publish().RefCount();

        IObservable<bool> optionsChanged = setting.Value.CombineLatest(newOptions).Select(tuple =>
            {
                (GeneralOptions oldGeneralOptions, GeneralOptions newGeneralOptions) = tuple;
                bool result = oldGeneralOptions != newGeneralOptions;
                return result;
            }).ObserveOn(schedulerProvider.Dispatcher)
            .Publish()
            .RefCount();

        Save = ReactiveCommand.Create(() => { }, optionsChanged);

        _ = Save.WithLatestFrom(newOptions, (_, options) => options)
            .Subscribe(setting.Write);
    }

    [Reactive] public Theme SelectedTheme { get; set; } = Theme.Dark;

    [Reactive] public int AnalysisTimeout { get; set; } = 0;

    [Reactive] public bool LoadFilesAlreadyInQueue { get; set; } = false;

    [Reactive] public string TemporalPath { get; set; } = string.Empty;

    [Reactive] public ReadOnlyObservableCollection<FileFilter> FileFilters { get; set; }

    [Reactive] public ReadOnlyObservableCollection<string> SupportedExtensions { get; set; }

    public IEnumerable<Theme> AppThemes { get; } = EnumUtils.GetValues<Theme>();

    public IEnumerable<LanguageModel> Languages { get; }

    [Reactive] public LanguageModel SelectedLanguage { get; set; }

    public ReactiveCommand<Unit, Unit> Save { get; set; }

    public ViewModelActivator Activator { get; }

    private IObservable<GeneralOptions> NewOptionObservable()
    {
        var filters = _fileFilters.Connect().ToCollection().StartWithEmpty();
        var extensions = _supportedExtensions.Connect().ToCollection();

        return Observable.CombineLatest(
            this.WhenAnyValue(x => x.SelectedTheme),
            this.WhenAnyValue(x => x.SelectedLanguage),
            this.WhenAnyValue(x => x.AnalysisTimeout),
            this.WhenAnyValue(x => x.LoadFilesAlreadyInQueue),
            this.WhenAnyValue(x => x.TemporalPath),
            filters,
            extensions,
            (theme, language, timeout, loadFiles, temporalPath, fileFilters, supportedExtensions) => new GeneralOptions(
                theme, language.Code, timeout, temporalPath, supportedExtensions.AsArray(),
                fileFilters.AsArray(), loadFiles));
    }

    private void HandleActivation(ISchedulerProvider schedulerProvider, ISetting<GeneralOptions> setting,
        CompositeDisposable disposable)
    {
        _ = setting.Value
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(options => SelectedTheme = options.Theme)
            .DisposeWith(disposable);

        _ = setting.Value
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(options => SelectedLanguage = Languages.First(x => x.Code == options.Language))
            .DisposeWith(disposable);

        _ = setting.Value
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(options => AnalysisTimeout = options.AnalysisTimeout)
            .DisposeWith(disposable);

        _ = setting.Value
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(options => LoadFilesAlreadyInQueue = options.LoadFilesAlreadyInQueue)
            .DisposeWith(disposable);

        _ = setting.Value
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(options => TemporalPath = options.TemporalFolder)
            .DisposeWith(disposable);

        _ = setting.Value
            .Subscribe(options =>
            {
                _fileFilters.Edit(cache =>
                {
                    cache.Clear();
                    cache.AddRange(options.FileFilters);
                });
            })
            .DisposeWith(disposable);

        _ = setting.Value
            .Subscribe(options =>
            {
                _supportedExtensions.Edit(cache =>
                {
                    cache.Clear();
                    cache.AddRange(options.SupportedFilesExtensions);
                });
            })
            .DisposeWith(disposable);
    }

    private void HandleDeactivation()
    {
        _fileFilters.Clear();
        _supportedExtensions.Clear();
    }
}
