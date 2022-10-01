using System;
using Splat;

namespace MultiConverter.DependencyInjection;

public static class ReadonlyDependencyResolverExtensions
{
    public static TService GetRequiredService<TService>(this IReadonlyDependencyResolver resolver)
    {
        TService? service = resolver.GetService<TService>();
        if (service is null)
        {
            throw new InvalidOperationException($"Failed to resolve object of type {typeof(TService)}");
        }

        return service;
    }

    public static object GetRequiredService(this IReadonlyDependencyResolver resolver, Type type)
    {
        object? service = resolver.GetService(type);
        if (service is null)
        {
            throw new InvalidOperationException($"Failed to resolve object of type {type}");
        }

        return service;
    }
}
