using System;
using MultiConverter.Models.Settings.General;

namespace MultiConverter.ViewModels.Options.Interfaces;

public interface IOptionItem
{
    IObservable<bool> HasChanged { get; }

    Func<GeneralOptions, GeneralOptions> UpdateOption { get; }
}
