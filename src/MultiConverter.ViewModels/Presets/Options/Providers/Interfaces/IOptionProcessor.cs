using System;
using MultiConverter.Models.Presets.Interfaces;

namespace MultiConverter.ViewModels.Presets.Options.Providers.Interfaces;

public interface IOptionProcessor<out T> where T : IOption
{
    Type OptionType { get; }
    OptionGeneratorBase Generate();
}
