using MultiConverter.Models.Presets;

namespace MultiConverter.ViewModels.Presets.Interfaces;

public interface IPostConversionViewModelFactory
{
    InputPostConversionViewModel Build(InputPostConversion presetPostConversion);
}
