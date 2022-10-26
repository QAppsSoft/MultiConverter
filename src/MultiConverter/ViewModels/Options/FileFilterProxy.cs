using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using MultiConverter.Models.Settings.General.FileFilters;
using MultiConverter.Utils;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Options;

public sealed class FileFilterProxy : ReactiveObject, IDisposable
{
    public FileFilterProxy() : this(FileFilter.Default)
    {

    }

    public FileFilterProxy(FileFilter fileFilter)
    {
        Filter = fileFilter.Filter;
        Position = fileFilter.Position;
        ApplyOn = fileFilter.ApplyOn;

        Editing = fileFilter == FileFilter.Default;

        var isNewObservable = Observable.Return(fileFilter == FileFilter.Default);

        var filterChanged = this.WhenAnyValue(x => x.Filter).Select(filter => filter != fileFilter.Filter);
        var positionChanged = this.WhenAnyValue(x => x.Position).Select(position => position != fileFilter.Position);
        var applyOnChanged = this.WhenAnyValue(x => x.ApplyOn).Select(applyOn => applyOn != fileFilter.ApplyOn);

        IObservable<bool> hasChanged = Observable.CombineLatest(filterChanged, positionChanged, applyOnChanged, isNewObservable)
            .Select(values => values.Any(x => x));

        hasChanged.ToPropertyEx(this, vm => vm.HasChanged);

        ToggleEditing = ReactiveCommand.Create(() => { Editing = !Editing; });
    }

    [Reactive] public string Filter { get; set; }

    [Reactive] public FileFilterPosition Position { get; set; }

    public IEnumerable<FileFilterPosition> Positions { get; } = EnumUtils.GetValues<FileFilterPosition>();

    [Reactive] public  FileFilterApplyOn ApplyOn { get; set; }

    public IEnumerable<FileFilterApplyOn> ApplyOnParts { get; } = EnumUtils.GetValues<FileFilterApplyOn>();

    [ObservableAsProperty] public bool HasChanged { get; }

    [Reactive] public bool Editing { get; private set; }

    public ReactiveCommand<Unit, Unit> ToggleEditing { get; }

    public static implicit operator FileFilter(FileFilterProxy filterProxy) =>
        new(filterProxy.Filter, filterProxy.Position, filterProxy.ApplyOn);

    public void Dispose() => ToggleEditing.Dispose();
}
