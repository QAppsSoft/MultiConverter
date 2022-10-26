
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

        var hasChanged = supportedExtensionsObservable
            .Filter(x => x.HasChanged)
            .IsNotEmpty();

        hasChanged.ToPropertyEx(this, vm => vm.HasChanged);

        UpdateOption = option =>
            option with { SupportedFilesExtensions = SupportedExtensions.Select(x => (string)x).AsArray() };

        var anyExtension = _supportedExtensions.Connect().IsNotEmpty();

        Delete = ReactiveCommand.Create<string>(extension => _supportedExtensions.Remove(extension), anyExtension);

        Reset = ReactiveCommand.Create(() => _supportedExtensions.Edit(cache =>
        {
            cache.Clear();
            cache.AddRange(GeneralOptions.Default().SupportedFilesExtensions);
        }));

        var canAdd = supportedExtensionsObservable.Filter(x => x == string.Empty).IsEmpty();

        Add = ReactiveCommand.Create(() => _supportedExtensions.Add(string.Empty), canAdd);

        _cleanup = new CompositeDisposable(updateExtensions, bindExtensions, supportedExtensionsObservable.Connect());
    }

    public ReadOnlyObservableCollection<ExtensionProxy> SupportedExtensions { get; }

    public ReactiveCommand<string, Unit> Delete { get; }

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
