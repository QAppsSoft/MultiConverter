using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using MultiConverter.Common;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.Utils;
using MultiConverter.ViewModels.Options.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Options;

public sealed class ThemeOptionItem : ViewModelBase, IOptionItem, IDisposable
{
    private readonly IDisposable _cleanup;

    public ThemeOptionItem(ISchedulerProvider schedulerProvider, ISetting<GeneralOptions> setting)
    {
        IDisposable updateSavedTheme = setting.Value
            .Select(x => x.Theme)
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(theme => SelectedTheme = theme);

        IObservable<Theme> newSelectedTheme = this.WhenAnyValue(x => x.SelectedTheme);

        HasChanged = setting.Value
            .Select(x => x.Theme)
            .CombineLatest(newSelectedTheme)
            .Select(tuple => tuple.First != tuple.Second);

        UpdateOption = option => option with { Theme = SelectedTheme };

        _cleanup = updateSavedTheme;
    }

    [Reactive] public Theme SelectedTheme { get; set; }

    public IEnumerable<Theme> Themes { get; } = EnumUtils.GetValues<Theme>();

    public void Dispose() => _cleanup.Dispose();

    public IObservable<bool> HasChanged { get; }

    public Func<GeneralOptions, GeneralOptions> UpdateOption { get; }
}
