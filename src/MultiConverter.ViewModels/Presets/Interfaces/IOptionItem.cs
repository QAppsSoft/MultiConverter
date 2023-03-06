using System.ComponentModel;

namespace MultiConverter.ViewModels.Presets.Interfaces;

public interface IOptionItem : INotifyPropertyChanged
{
    bool HasChanged { get; }
}
