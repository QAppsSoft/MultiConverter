using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using MultiConverter.Common;
using MultiConverter.Extensions;
using MultiConverter.Models.Presets.Output;
using MultiConverter.ViewModels.Presets.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Output;

public sealed class OutputTemplateViewModel : ViewModelBase, IChanged, IDisposable
{
    private readonly CompositeDisposable _cleanup = new();

    public OutputTemplateViewModel(OutputPathTemplate outputPathTemplate, ISchedulerProvider schedulerProvider)
    {
        ArgumentNullException.ThrowIfNull(outputPathTemplate);
        ArgumentNullException.ThrowIfNull(schedulerProvider);

        InitializeProperties(outputPathTemplate);

        HasChangedObservable(outputPathTemplate).ToPropertyEx(this, vm => vm.HasChanged).DisposeWith(_cleanup);
    }

    [Reactive] public string Template { get; set; }

    [Reactive] public bool OverrideContainerExtension { get; set; }

    [Reactive] public string OutputExtension { get; set; }

    [ObservableAsProperty] public bool HasChanged { get; }

    private void InitializeProperties(OutputPathTemplate outputPathTemplate)
    {
        Template = outputPathTemplate.Template;
        OverrideContainerExtension = outputPathTemplate.OverrideContainerExtension;
        OutputExtension = outputPathTemplate.OutputExtension;
    }

    private IObservable<bool> HasChangedObservable(OutputPathTemplate outputPathTemplate) =>
        new[]
        {
            this.WhenAnyValue(vm => vm.Template).Select(template => template != outputPathTemplate.Template),
            this.WhenAnyValue(vm => vm.OverrideContainerExtension).Select(overrideExtension => overrideExtension != outputPathTemplate.OverrideContainerExtension),
            this.WhenAnyValue(vm => vm.OutputExtension).Select(extension => extension != outputPathTemplate.OutputExtension)
        }.CombineLatest(values => values.AnyIsTrue());

    public static implicit operator OutputPathTemplate(OutputTemplateViewModel vm) =>
        new(vm.Template, vm.OverrideContainerExtension, vm.OutputExtension);

    public void Dispose() => _cleanup.Dispose();
}
