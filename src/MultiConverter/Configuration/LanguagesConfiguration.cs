using System.Collections.Generic;

namespace MultiConverter.Configuration;

public sealed class LanguagesConfiguration
{
    public List<string> AvailableLocales { get; init; } = null!;
}
