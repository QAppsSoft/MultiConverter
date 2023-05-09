using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using DynamicData.Binding;
using DynamicData.Kernel;

namespace MultiConverter.ViewModels.Extensions;

public static class CollectionsEx
{
    /// <summary>
    ///     Cast an non generic IEnumerable to his items
    /// </summary>
    /// <param name="source">IList source</param>
    /// <typeparam name="T">Type of internal objects</typeparam>
    /// <returns></returns>
    private static IEnumerable<T> Cast<T>(this IEnumerable source) => Enumerable.Cast<T>(source);

    /// <summary>
    ///     Observe collection changes and return an strongly typed tuple of added and removed items
    /// </summary>
    /// <param name="source">Source observable collection</param>
    /// <typeparam name="T">Type of observable collection items</typeparam>
    /// <returns>Tuple of added and removed items</returns>
    public static IObservable<(Optional<IEnumerable<T>> NewItems, Optional<IEnumerable<T>> OldItems)>
        ObserveCollectionChangesOptional<T>(this INotifyCollectionChanged source) =>
        source.ObserveCollectionChanges().Select(eventPattern =>
        {
            Optional<IEnumerable<T>> newItems = Optional<IEnumerable<T>>.None;
            Optional<IEnumerable<T>> oldItems = Optional<IEnumerable<T>>.None;

            if (eventPattern.EventArgs.NewItems?.Count > 0)
            {
                newItems = Optional<IEnumerable<T>>.Create(eventPattern.EventArgs.NewItems.Cast<T>().AsArray());
            }

            if (eventPattern.EventArgs.OldItems?.Count > 0)
            {
                oldItems = Optional<IEnumerable<T>>.Create(eventPattern.EventArgs.OldItems.Cast<T>().AsArray());
            }

            return (newItems, oldItems);
        });
}
