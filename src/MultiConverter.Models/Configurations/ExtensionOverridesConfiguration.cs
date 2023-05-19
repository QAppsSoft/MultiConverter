using System.Collections.Generic;

namespace MultiConverter.Models.Configurations;

public sealed class ExtensionOverridesConfiguration
{
    public Dictionary<string, string> Overrides { get; init; } = null!;
}
