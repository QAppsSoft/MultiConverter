using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Aggregation;
using DynamicData.Kernel;
using MultiConverter.Common;
using MultiConverter.Models.Settings.General;
using MultiConverter.Models.Settings.General.FileFilters;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels.Options.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Options;

public sealed class FileFiltersOptionItem : ViewModelBase, IOptionItem, IDisposable
{
    private readonly IDisposable _cleanup;

    private readonly ISourceList<FileFilter> _filters = new SourceList<FileFilter>();

    public FileFiltersOptionItem(ISchedulerProvider schedulerProvider, ISetting<GeneralOptions> setting)
    {
        var updateFilters = setting.Value
            .Select(x => x.FileFilters)
            .Subscribe(filters =>
            {
                _filters.Edit(cache =>
                {
                    cache.Clear();
                    cache.AddRange(filters);
                });
            });

        var fileFiltersObservable = _filters.Connect()
            .Transform(x => new FileFilterProxy(x))
            .DisposeMany()
            .AutoRefresh(x => x.HasChanged)
            .Publish();

        var bindFilters = fileFiltersObservable
            .ObserveOn(schedulerProvider.Dispatcher)
            .Bind(out var fileFilters)
            .Subscribe();

        FileFilters = fileFilters;

        IObservable<bool> hasChanged = fileFiltersObservable
            .Filter(x => x.HasChanged)
            .IsNotEmpty();

        hasChanged.ToPropertyEx(this, vm => vm.HasChanged);

        UpdateOption = option => option with { FileFilters = FileFilters.Select(x => (FileFilter)x).AsArray() };

        var anyFilter = _filters.Connect().IsNotEmpty();

        Delete = ReactiveCommand.Create<FileFilter>(filter => _filters.Remove(filter), anyFilter);

        Reset = ReactiveCommand.Create(() => _filters.Edit(cache =>
        {
            cache.Clear();
            cache.AddRange(GeneralOptions.Default().FileFilters);
        }));

        var canAdd = fileFiltersObservable.Filter(x => x == FileFilter.Default)
            .IsEmpty();

        Add = ReactiveCommand.Create(() => _filters.Add(FileFilter.Default), canAdd);

        _cleanup = new CompositeDisposable(updateFilters, bindFilters, fileFiltersObservable.Connect());
    }

    public ReadOnlyObservableCollection<FileFilterProxy> FileFilters { get; }

    public ReactiveCommand<FileFilter, Unit> Delete { get; }

    public ReactiveCommand<Unit, Unit> Add { get; }

    public ReactiveCommand<Unit, Unit> Reset { get; }

    [ObservableAsProperty] public bool HasChanged { get; }

    public Func<GeneralOptions, GeneralOptions> UpdateOption { get; }


    public void Dispose()
    {
        _cleanup.Dispose();
        _filters.Dispose();
        Delete.Dispose();
        Add.Dispose();
        Reset.Dispose();
    }
}
