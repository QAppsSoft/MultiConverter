using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Interfaces;

public interface IOptionsViewModelFactory
{
    OptionsViewModel Build(IOption[] options);

    OptionViewModelBase Build(IOption option);
}
