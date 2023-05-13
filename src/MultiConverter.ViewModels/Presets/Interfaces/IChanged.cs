using System.ComponentModel;

namespace MultiConverter.ViewModels.Presets.Interfaces;

public interface IChanged : INotifyPropertyChanged
{
    bool HasChanged { get; }
}
