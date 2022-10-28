using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Binding;
using DynamicData.Kernel;
using MultiConverter.Common;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Options.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Options;

public sealed class SupportedFileExtensionOptionItem : ViewModelBase, IOptionItem, IDisposable
{
    private readonly IDisposable _cleanup;

    private readonly ISourceList<string> _supportedExtensions = new SourceList<string>();

    public SupportedFileExtensionOptionItem(ISchedulerProvider schedulerProvider, ISetting<GeneralOptions> setting)
    {
        var updateExtensions = setting.Value
            .Select(x => x.SupportedFilesExtensions)
            .Subscribe(extensions =>
            {
                _supportedExtensions.Edit(cache =>
                {
                    cache.Clear();
                    cache.AddRange(extensions);
                });
            });

        var supportedExtensionsObservable = _supportedExtensions.Connect()
            .Sort(SortExpressionComparer<string>.Ascending(x => x))
            .Transform(x => new ExtensionProxy(x))
            .AutoRefresh(x => x.HasChanged)
            .Publish();

        var bindExtensions = supportedExtensionsObservable
            .ObserveOn(schedulerProvider.Dispatcher)
            .Bind(out var supportedExtensions)
            .Subscribe();

        SupportedExtensions = supportedExtensions;

        var hasChanged = LocalListIsChanged(setting, supportedExtensionsObservable);

        hasChanged.ToPropertyEx(this, vm => vm.HasChanged);

        UpdateOption = option =>
            option with { SupportedFilesExtensions = SupportedExtensions.Select(x => (string)x).AsArray() };

        var anyExtension = _supportedExtensions.Connect().IsNotEmpty();

        Delete = ReactiveCommand.Create<ExtensionProxy>(extension => _supportedExtensions.Remove(extension), anyExtension);

        Reset = ReactiveCommand.Create(() => _supportedExtensions.Edit(cache =>
        {
            cache.Clear();
            cache.AddRange(GeneralOptions.Default().SupportedFilesExtensions);
        }));

        var canAdd = supportedExtensionsObservable.Filter(x => x == string.Empty).IsEmpty();

        Add = ReactiveCommand.Create(() => _supportedExtensions.Add(string.Empty), canAdd);

        _cleanup = new CompositeDisposable(updateExtensions, bindExtensions, supportedExtensionsObservable.Connect());
    }

    private IObservable<bool> LocalListIsChanged(ISetting<GeneralOptions> setting, IObservable<IChangeSet<ExtensionProxy>> supportedExtensionsObservable)
    {
        var extensionsCountChanged = ExtensionsCountChangedObservable(setting, _supportedExtensions);

        var anyExtensionEdited = IsAnyExtensionEdited(supportedExtensionsObservable);

        var hasChanged = extensionsCountChanged.CombineLatest(anyExtensionEdited, (first, second) => first || second);

        return hasChanged;
    }

    private static IObservable<bool> IsAnyExtensionEdited(IObservable<IChangeSet<ExtensionProxy>> supportedExtensionsObservable) =>
        supportedExtensionsObservable
            .Filter(x => x.HasChanged)
            .IsNotEmpty();

    private static IObservable<bool> ExtensionsCountChangedObservable(ISetting<GeneralOptions> setting, IObservableList<string> supportedExtensions)
    {
        IObservable<int> settingCount = setting.Value
            .Select(x => x.SupportedFilesExtensions)
            .Select(x => x.Length).StartWith(0);

        IObservable<int> localCount = supportedExtensions.Connect()
            .Count()
            .StartWith(0);

        IObservable<bool> extensionsCountChanged = settingCount
            .CombineLatest(localCount, (first, second) => first != second);

        return extensionsCountChanged;
    }

    public ReadOnlyObservableCollection<ExtensionProxy> SupportedExtensions { get; }

    public ReactiveCommand<ExtensionProxy, Unit> Delete { get; }

    public ReactiveCommand<Unit, Unit> Add { get; }

    public ReactiveCommand<Unit, Unit> Reset { get; }

    [ObservableAsProperty] public bool HasChanged { get; }
    public Func<GeneralOptions, GeneralOptions> UpdateOption { get; }

    public void Dispose()
    {
        _cleanup.Dispose();
        _supportedExtensions.Dispose();
        Delete.Dispose();
        Add.Dispose();
        Reset.Dispose();
    }
}
