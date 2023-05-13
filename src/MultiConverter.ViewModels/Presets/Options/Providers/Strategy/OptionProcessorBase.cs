using System;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Options.Providers.Interfaces;

namespace MultiConverter.ViewModels.Presets.Options.Providers.Strategy;

public abstract class OptionProcessorBase<T> : IOptionProcessor<T> where T : IOption
{
    public Type OptionType { get; } = typeof(T);
    public abstract OptionGeneratorBase Generate();
}
