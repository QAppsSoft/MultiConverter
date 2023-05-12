using System;
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

    /// <summary>
    ///     Execute an action foreach IEnumerable item
    /// </summary>
    /// <param name="source">Items source</param>
    /// <param name="action">Action to execute</param>
    /// <typeparam name="T">Items type</typeparam>
    /// <exception cref="ArgumentNullException">Throw if source or action parameters are null</exception>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in source)
        {
            action(item);
        }
    }
}
