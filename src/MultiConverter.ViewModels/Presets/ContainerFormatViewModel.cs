using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Formats;
using MultiConverter.Services.Abstractions.Formats;
using MultiConverter.ViewModels.Presets.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets;

public class ContainerFormatViewModel : ViewModelBase, IChanged
{
    public ContainerFormatViewModel(string containerFormat, IContainersFormatProvider formatProvider, ISchedulerProvider schedulerProvider)
    {
        Formats = formatProvider.Formats();
        SelectedFormat = Formats.First(f => f.Name == containerFormat);

        this.WhenAnyValue(vm => vm.SelectedFormat)
            .Select(f => f.Name != containerFormat)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, vm => vm.HasChanged);
    }

    public IEnumerable<ContainerFormat> Formats { get; }

    [Reactive] public ContainerFormat SelectedFormat { get; set; }
    [ObservableAsProperty] public bool HasChanged { get; }

    public static implicit operator string(ContainerFormatViewModel vm) => vm.SelectedFormat.Name;
}
