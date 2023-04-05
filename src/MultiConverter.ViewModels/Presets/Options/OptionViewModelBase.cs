using System;
using System.Reactive;
using System.Reactive.Linq;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Options;

public abstract class OptionViewModelBase : ViewModelBase, IOptionItem
{
    protected OptionViewModelBase(ISchedulerProvider schedulerProvider)
    {
        _ = this.WhenAnyValue(x => x.DefaultOptions)
            .Select(x => x.Length > 0)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, x => x.HasValues);

        UpdateValues = ReactiveCommand.Create<ValuesUpdater>(
            value => value.Update(),
            this.WhenAnyValue(x => x.HasValues));
    }

    public ReactiveCommand<ValuesUpdater, Unit> UpdateValues { get; }

    [Reactive] public ValuesUpdater[] DefaultOptions { get; protected set; } = Array.Empty<ValuesUpdater>();

    [ObservableAsProperty] public bool HasValues { get; }

    [ObservableAsProperty] public bool HasChanged { get; }

    public abstract IOption GetOption();
}

public sealed class ValuesUpdater
{
    public string Caption { get; init; } = string.Empty;
    public Action Update { get; init; } = null!;
}
