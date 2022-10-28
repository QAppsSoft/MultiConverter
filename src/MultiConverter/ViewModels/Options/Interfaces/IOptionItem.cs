using System;
using System.ComponentModel;
using MultiConverter.Models.Settings.General;

namespace MultiConverter.ViewModels.Options.Interfaces;

public interface IOptionItem : INotifyPropertyChanged
{
    bool HasChanged { get; }

    Func<GeneralOptions, GeneralOptions> UpdateOption { get; }
}
