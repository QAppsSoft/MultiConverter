using System.Collections.Generic;

namespace MultiConverter.Models.Configurations;

public sealed class FavoriteFormatsConfiguration
{
    public List<string> Favorites { get; init; } = null!;
}
