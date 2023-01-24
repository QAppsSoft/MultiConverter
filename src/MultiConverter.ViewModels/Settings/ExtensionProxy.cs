using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Settings;

public sealed class ExtensionProxy : ReactiveObject
{
    public ExtensionProxy() : this(string.Empty)
    {

    }

    public ExtensionProxy(string extension)
    {
        Extension = extension;

        Editing = extension == string.Empty;

        var isNewObservable = Observable.Return(extension == string.Empty);

        var extensionChanged = this.WhenAnyValue(x => x.Extension)
            .Select(ext => ext != extension);

        IObservable<bool> hasChanged = Observable.CombineLatest(extensionChanged, isNewObservable)
            .Select(values => values.Any(x => x));

        hasChanged.ToPropertyEx(this, vm => vm.HasChanged);

        ToggleEditing = ReactiveCommand.Create(() => { Editing = !Editing; });
    }

    [Reactive] public string Extension { get; set; }

    [ObservableAsProperty] public bool HasChanged { get; }

    [Reactive] public bool Editing { get; private set; }

    public ReactiveCommand<Unit, Unit> ToggleEditing { get; }

    public static implicit operator string(ExtensionProxy extensionProxy) => extensionProxy.Extension;

}
