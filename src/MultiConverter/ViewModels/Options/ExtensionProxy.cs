﻿using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Options;

public sealed class ExtensionProxy
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

        HasChanged = Observable.CombineLatest(extensionChanged, isNewObservable)
            .Select(values => values.Any(x => x));

        ToggleEditing = ReactiveCommand.Create(() => { Editing = !Editing; });
    }

    [Reactive] public string Extension { get; set; }

    public IObservable<bool> HasChanged { get; }

    [Reactive] public bool Editing { get; private set; }

    public ReactiveCommand<Unit, Unit> ToggleEditing { get; }

    public static implicit operator string(ExtensionProxy extensionProxy) => extensionProxy.Extension;

}