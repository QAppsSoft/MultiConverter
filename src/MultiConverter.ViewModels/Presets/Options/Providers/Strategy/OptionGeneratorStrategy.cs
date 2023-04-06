using System;
using System.Collections.Generic;
using System.Linq;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Options.Providers.Interfaces;

namespace MultiConverter.ViewModels.Presets.Options.Providers.Strategy;

public class OptionGeneratorStrategy : IOptionGeneratorStrategy
{
    private readonly IEnumerable<IOptionProcessor<IOption>> _operators;

    public OptionGeneratorStrategy(IEnumerable<IOptionProcessor<IOption>> operators)
    {
        ArgumentNullException.ThrowIfNull(operators);

        _operators = operators;
    }

    public OptionGeneratorBase Generate(Type optionType) =>
        _operators
            .First(x => x.OptionType == optionType)
            .Generate();
}
