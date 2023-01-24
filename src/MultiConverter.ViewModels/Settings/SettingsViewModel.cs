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
using MultiConverter.ViewModels.Settings.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Settings;

public sealed class SettingsViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly SourceList<ISettingItem> _optionsSourceList = new();

    public SettingsViewModel(ISchedulerProvider schedulerProvider,
        ISetting<GeneralOptions> setting, IEnumerable<ISettingItem> optionItems)
    {
        ArgumentNullException.ThrowIfNull(schedulerProvider);
        ArgumentNullException.ThrowIfNull(optionItems);
        ArgumentNullException.ThrowIfNull(setting);

        _optionsSourceList.AddRange(optionItems);

        this.WhenActivated(disposable =>
        {
            HandleActivation();

            Disposable.Create(HandleDeactivation).DisposeWith(disposable);

            _ = _optionsSourceList.Connect()
                .ObserveOn(schedulerProvider.Dispatcher)
                .Bind(out ReadOnlyObservableCollection<ISettingItem> options)
                .Subscribe()
                .DisposeWith(disposable);

            Options = options;

            IObservable<bool> hasOptionsChanged = _optionsSourceList.Connect()
                .AutoRefresh(x => x.HasChanged)
                .Filter(x => x.HasChanged)
                .IsNotEmpty()
                .StartWith(false);

            Save = ReactiveCommand.Create(() => { }, hasOptionsChanged);

            IObservable<IReadOnlyCollection<ISettingItem>> changedOptions = _optionsSourceList.Connect()
                .AutoRefresh(x => x.HasChanged)
                .Filter(x => x.HasChanged)
                .ToCollection()
                .StartWithEmpty();

            IObservable<(GeneralOptions First, IReadOnlyCollection<ISettingItem> Second)> optionsTuple =
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
        Save = null;
        Reset?.Dispose();
        Reset = null;
    }

    private void HandleActivation()
    {
        // Empty
    }

    [Reactive] public ReadOnlyObservableCollection<ISettingItem>? Options { get; set; }

    [Reactive] public ReactiveCommand<Unit, Unit>? Save { get; set; }

    [Reactive] public ReactiveCommand<Unit, Unit>? Reset { get; set; }

    public ViewModelActivator Activator { get; } = new();

    private static GeneralOptions NewGeneralOptions(
        (GeneralOptions GeneralOptions, IReadOnlyCollection<ISettingItem> ChangedOptions) tuple)
    {
        (GeneralOptions generalOptions, IReadOnlyCollection<ISettingItem> readOnlyCollection) = tuple;

        foreach (ISettingItem optionItem in readOnlyCollection)
        {
            generalOptions = optionItem.UpdateOption(generalOptions);
        }

        return generalOptions;
    }
}
