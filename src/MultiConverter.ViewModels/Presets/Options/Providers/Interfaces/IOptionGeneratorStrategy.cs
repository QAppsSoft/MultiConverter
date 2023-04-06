using System;

namespace MultiConverter.ViewModels.Presets.Options.Providers.Interfaces;

public interface IOptionGeneratorStrategy
{
    OptionGeneratorBase Generate(Type optionType);
}
