using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Aggregation;
using MultiConverter.Common;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Options.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Options;

public sealed class OptionsViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly SourceList<IOptionItem> _fileFilters = new();

    public OptionsViewModel(ISchedulerProvider schedulerProvider,
        ISetting<GeneralOptions> setting, IEnumerable<IOptionItem> optionItems)
    {
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(optionItems);
        ArgumentNullException.ThrowIfNull(setting);

        _fileFilters.AddRange(optionItems);

        this.WhenActivated(disposable =>
        {
            HandleActivation();

            Disposable.Create(HandleDeactivation).DisposeWith(disposable);

            _ = _fileFilters.Connect()
                .DisposeMany()
                .ObserveOn(schedulerProvider.Dispatcher)
                .Bind(out ReadOnlyObservableCollection<IOptionItem> options)
                .Subscribe()
                .DisposeWith(disposable);

            Options = options;

            IObservable<bool> hasOptionsChanged = _fileFilters.Connect()
                .AutoRefresh(x => x.HasChanged)
                .Filter(x => x.HasChanged)
                .IsNotEmpty()
                .StartWith(false);

            Save = ReactiveCommand.Create(() => { }, hasOptionsChanged);

            IObservable<IReadOnlyCollection<IOptionItem>> changedOptions = _fileFilters.Connect()
                .AutoRefresh(x => x.HasChanged)
                .Filter(x => x.HasChanged)
                .ToCollection()
                .StartWithEmpty();

            IObservable<(GeneralOptions First, IReadOnlyCollection<IOptionItem> Second)> optionsTuple =
                setting.Value.CombineLatest(changedOptions);

            _ = Save.WithLatestFrom(optionsTuple).Select(x => x.Second)
                .Select(NewGeneralOptions)
                .Subscribe(setting.Write)
                .DisposeWith(disposable);

            Reset = ReactiveCommand.Create(() => setting.Write(GeneralOptions.Default()));
        });
    }

    private void HandleDeactivation()
    {
        Save?.Dispose();
        Reset?.Dispose();
    }

    private void HandleActivation()
    {
        // Empty
    }

    [Reactive] public ReadOnlyObservableCollection<IOptionItem>? Options { get; set; }

    [Reactive] public ReactiveCommand<Unit, Unit>? Save { get; set; }

    [Reactive] public ReactiveCommand<Unit, Unit>? Reset { get; set; }

    public ViewModelActivator Activator { get; } = new();

    private static GeneralOptions NewGeneralOptions(
        (GeneralOptions GeneralOptions, IReadOnlyCollection<IOptionItem> ChangedOptions) tuple)
    {
        (GeneralOptions generalOptions, IReadOnlyCollection<IOptionItem> readOnlyCollection) = tuple;

        foreach (IOptionItem optionItem in readOnlyCollection)
        {
            generalOptions = optionItem.UpdateOption(generalOptions);
        }

        return generalOptions;
    }
}
