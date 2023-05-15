using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Formats;
using MultiConverter.Services.Abstractions.Formats;
using MultiConverter.ViewModels.Presets.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets;

public sealed class ContainerFormatViewModel : ViewModelBase, IChanged, IDisposable
{
    private readonly CompositeDisposable _cleanup = new();

    public ContainerFormatViewModel(string containerFormat, IContainersFormatProvider formatProvider, ISchedulerProvider schedulerProvider)
    {
        Formats = formatProvider.Formats();
        SelectedFormat = Formats.First(f => f.Name == containerFormat);

        this.WhenAnyValue(vm => vm.SelectedFormat)
            .Select(f => f.Name != containerFormat)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, vm => vm.HasChanged)
            .DisposeWith(_cleanup);
    }

    public IEnumerable<ContainerFormat> Formats { get; }

    [Reactive] public ContainerFormat SelectedFormat { get; set; }
    [ObservableAsProperty] public bool HasChanged { get; }

    public static implicit operator string(ContainerFormatViewModel vm) => vm.SelectedFormat.Name;

    public void Dispose() => _cleanup.Dispose();
}
