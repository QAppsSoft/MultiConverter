using System.Collections.Generic;

namespace MultiConverter.ViewModels.Presets.Options.Providers.Interfaces;

public interface IPresetOptionsProvider
{
    IEnumerable<OptionGeneratorBase> Options { get; }
}
