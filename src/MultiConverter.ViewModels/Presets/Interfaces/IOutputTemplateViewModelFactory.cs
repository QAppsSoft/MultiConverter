using MultiConverter.Models.Presets.Output;
using MultiConverter.ViewModels.Presets.Output;

namespace MultiConverter.ViewModels.Presets.Interfaces;

public interface IOutputTemplateViewModelFactory
{
    OutputTemplateViewModel Build(OutputPathTemplate outputPathTemplate);
}
