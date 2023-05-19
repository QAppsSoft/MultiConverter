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

    /// <summary>
    ///     Filter any value received from the observable allowing only true values
    /// </summary>
    /// <param name="observable">The observable to filter</param>
    /// <returns><see cref="bool" /> value</returns>
    public static IObservable<bool> WhereTrue(this IObservable<bool> observable) => observable.Where(x => x);

    /// <summary>
    ///     Filter any value received from the observable allowing only false values
    /// </summary>
    /// <param name="observable">The observable to filter</param>
    /// <returns><see cref="bool" /> value</returns>
    public static IObservable<bool> WhereFalse(this IObservable<bool> observable) => observable.Where(x => !x);

    /// <summary>
    ///     Return true for any int greater than value
    /// </summary>
    /// <param name="observable">The observable</param>
    /// <returns><see cref="bool" /> value</returns>
    public static IObservable<bool> GreaterThan(this IObservable<int> observable, int value) => observable.Select(x => x > value);

    /// <summary>
    ///     Return true for any int lower than value
    /// </summary>
    /// <param name="observable">The observable</param>
    /// <returns><see cref="bool" /> value</returns>
    public static IObservable<bool> LowerThan(this IObservable<int> observable, int value) => observable.Select(x => x < value);

    /// <summary>
    ///     Return true for any int equal value
    /// </summary>
    /// <param name="observable">The observable</param>
    /// <returns><see cref="bool" /> value</returns>
    public static IObservable<bool> EqualTo(this IObservable<int> observable, int value) => observable.Select(x => x == value);

    /// <summary>
    ///     Return true if null
    /// </summary>
    /// <param name="observable">The observable</param>
    public static IObservable<bool> IsNull<T>(this IObservable<T> observable) => observable.Select(x => x is null);

    /// <summary>
    ///     Return true if not null
    /// </summary>
    /// <param name="observable">The observable</param>
    public static IObservable<bool> IsNotNull<T>(this IObservable<T> observable) => observable.Select(x => x is not null);

    /// <summary>
    ///     Invert any received bool value
    /// </summary>
    /// <param name="observable"></param>
    /// <returns>An stream of bool with inverted values</returns>
    public static IObservable<bool> InvertValue(this IObservable<bool> observable) => observable.Select(value => !value);
}
