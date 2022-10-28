using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using MultiConverter.Common;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Options.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Options;

public sealed class VideoOptionItem : ViewModelBase, IOptionItem, IDisposable
{
    private readonly IDisposable _cleanup;

    public VideoOptionItem(ISchedulerProvider schedulerProvider, ISetting<GeneralOptions> setting)
    {
        var updateSavedTimeout = setting.Value
            .Select(x => x.AnalysisTimeout)
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(timeout => AnalysisTimeout = timeout);

        var updateLoadAlreadyInQueue = setting.Value
            .Select(x => x.LoadFilesAlreadyInQueue)
            .ObserveOn(schedulerProvider.Dispatcher)
            .Subscribe(inQueue => LoadFilesAlreadyInQueue = inQueue);

        var newTimeout = this.WhenAnyValue(x => x.AnalysisTimeout);

        var hasChangedTimeout = setting.Value
            .Select(x => x.AnalysisTimeout)
            .CombineLatest(newTimeout, (savedTimeout, newTimeout) => savedTimeout != newTimeout);

        var newInQueue = this.WhenAnyValue(x => x.LoadFilesAlreadyInQueue);

        var hasChangedInQueue = setting.Value
            .Select(x => x.LoadFilesAlreadyInQueue)
            .CombineLatest(newInQueue, (savedInQueue, newInQueue) => savedInQueue != newInQueue);

        var hasChanged = hasChangedTimeout.CombineLatest(hasChangedInQueue, (timeout, inQueue) => timeout || inQueue);

        hasChanged.ToPropertyEx(this, vm => vm.HasChanged);

        UpdateOption = option => option with
        {
            AnalysisTimeout = AnalysisTimeout,
            LoadFilesAlreadyInQueue = LoadFilesAlreadyInQueue
        };

        _cleanup = new CompositeDisposable(updateSavedTimeout, updateLoadAlreadyInQueue);
    }

    [Reactive] public int AnalysisTimeout { get; set; }

    [Reactive] public bool LoadFilesAlreadyInQueue { get; set; }

    [ObservableAsProperty] public bool HasChanged { get; }
    public Func<GeneralOptions, GeneralOptions> UpdateOption { get; }

    public void Dispose() => _cleanup.Dispose();
}
