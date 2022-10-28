using System;
using System.Reactive;
using System.Reactive.Linq;

namespace MultiConverter.Extensions;

public static class ObservableExtensions
{
    /// <summary>
    ///     Transform any value received from the observable to a single <see cref="Unit" /> value
    /// </summary>
    /// <param name="observable">The observable to transform</param>
    /// <typeparam name="T">observable values type</typeparam>
    /// <returns><see cref="Unit" /> value</returns>
    public static IObservable<Unit> ToUnit<T>(this IObservable<T> observable) => observable.Select(_ => Unit.Default);
}
