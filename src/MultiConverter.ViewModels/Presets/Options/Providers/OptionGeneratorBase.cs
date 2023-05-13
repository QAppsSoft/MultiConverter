using MultiConverter.Models.Presets.Interfaces;

namespace MultiConverter.ViewModels.Presets.Options.Providers;

public abstract class OptionGeneratorBase
{
    public abstract string Caption { get; init; }
    public abstract IOption Generate();
}
