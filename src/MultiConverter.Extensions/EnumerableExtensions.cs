using System.Collections.Generic;
using System.Linq;

namespace MultiConverter.Extensions;

public static class EnumerableExtensions
{
    /// <summary>
    ///     Check if any value is true
    /// </summary>
    /// <param name="source"></param>
    /// <returns><see cref="bool" /> Returns true if any is true</returns>
    public static bool AnyIsTrue(this IEnumerable<bool> source) => source.Any(value => value);
}
