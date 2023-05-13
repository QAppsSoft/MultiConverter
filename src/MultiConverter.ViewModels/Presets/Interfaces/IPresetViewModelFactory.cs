using MultiConverter.Models.Presets;

namespace MultiConverter.ViewModels.Presets.Interfaces;

public interface IPresetViewModelFactory
{
    PresetViewModel Build(Preset preset);
}
