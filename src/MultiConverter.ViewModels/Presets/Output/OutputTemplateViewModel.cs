using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MultiConverter.Common;
using MultiConverter.Extensions;
using MultiConverter.Models.Presets.Enums;
using MultiConverter.Models.Presets.Output;
using MultiConverter.Services.Abstractions.Dialogs;
using MultiConverter.ViewModels.Presets.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Output;

public sealed class OutputTemplateViewModel : ViewModelBase, IChanged, IDisposable
{
    private readonly CompositeDisposable _cleanup = new();

    public OutputTemplateViewModel(OutputPathTemplate outputPathTemplate, ISchedulerProvider schedulerProvider, IDialogService dialogService)
    {
        ArgumentNullException.ThrowIfNull(outputPathTemplate);
        ArgumentNullException.ThrowIfNull(schedulerProvider);

        InitializeProperties(outputPathTemplate);

        SelectFolder = ReactiveCommand.CreateFromTask(() => UpdateOutputPath(dialogService));

        HasChangedObservable(outputPathTemplate).ToPropertyEx(this, vm => vm.HasChanged).DisposeWith(_cleanup);
    }

    [Reactive] public OutputPathSelection OutputPathSelected { get; set; }

    [Reactive] public string FixedPath { get; set; }

    [Reactive] public string AddInPathCollision { get; set; }

    [Reactive] public string Template { get; set; }

    [Reactive] public bool OverrideContainerExtension { get; set; }

    [Reactive] public string OutputExtension { get; set; }

    [ObservableAsProperty] public bool HasChanged { get; }

    public OutputPathSelection[] OutputPathItems { get; } =
    {
        OutputPathSelection.SameAsInput, OutputPathSelection.FixedPath, OutputPathSelection.AdvancedTemplate
    };

    public ReactiveCommand<Unit, Unit> SelectFolder { get; }

    private async Task UpdateOutputPath(IDialogService dialogService)
    {
        FolderDialogSettings dialogSetting = new(FixedPath);

        string? fixedPath = await dialogService.ShowFolderSelectorAsync(dialogSetting);

        if (fixedPath == null)
        {
            return;
        }

        FixedPath = fixedPath;
    }

    private void InitializeProperties(OutputPathTemplate outputPathTemplate)
    {
        OutputPathSelected = outputPathTemplate.OutputPathSelection;
        Template = outputPathTemplate.Template;
        OverrideContainerExtension = outputPathTemplate.OverrideContainerExtension;
        OutputExtension = outputPathTemplate.OutputExtension;
        FixedPath = outputPathTemplate.FixedPath;
        AddInPathCollision = outputPathTemplate.AddInPathCollision;
    }

    private IObservable<bool> HasChangedObservable(OutputPathTemplate outputPathTemplate) =>
        new[]
        {
            this.WhenAnyValue(vm => vm.OutputPathSelected).Select(selection => selection != outputPathTemplate.OutputPathSelection),
            this.WhenAnyValue(vm => vm.Template).Select(template => template != outputPathTemplate.Template),
            this.WhenAnyValue(vm => vm.OverrideContainerExtension).Select(overrideExtension => overrideExtension != outputPathTemplate.OverrideContainerExtension),
            this.WhenAnyValue(vm => vm.OutputExtension).Select(extension => extension != outputPathTemplate.OutputExtension),
            this.WhenAnyValue(vm => vm.FixedPath).Select(fixedPath => fixedPath != outputPathTemplate.FixedPath),
            this.WhenAnyValue(vm => vm.AddInPathCollision).Select(inCollision => inCollision != outputPathTemplate.AddInPathCollision),
        }.CombineLatest(values => values.AnyIsTrue());

    public static implicit operator OutputPathTemplate(OutputTemplateViewModel vm) => new(
        vm.OutputPathSelected,
        vm.Template,
        vm.OverrideContainerExtension,
        vm.OutputExtension,
        vm.FixedPath,
        vm.AddInPathCollision
    );

    public void Dispose()
    {
        SelectFolder.Dispose();
        _cleanup.Dispose();
    }
}
