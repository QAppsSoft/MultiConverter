using MultiConverter.Models.Presets.Interfaces;

namespace MultiConverter.ViewModels.Presets.Interfaces;

public interface IOptionItem : IChanged
{
    IOption GetOption();
}
