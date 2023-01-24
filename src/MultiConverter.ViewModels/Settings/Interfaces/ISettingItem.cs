using System;
using System.ComponentModel;
using MultiConverter.Models.Settings.General;

namespace MultiConverter.ViewModels.Settings.Interfaces;

public interface ISettingItem : INotifyPropertyChanged
{
    bool HasChanged { get; }

    Func<GeneralOptions, GeneralOptions> UpdateOption { get; }
}
