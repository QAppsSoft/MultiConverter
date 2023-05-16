using System.Collections.Generic;

namespace MultiConverter.Models.Configurations;

public sealed class FavoriteCodecsConfiguration
{
    public List<string> Favorites { get; init; } = null!;
}
