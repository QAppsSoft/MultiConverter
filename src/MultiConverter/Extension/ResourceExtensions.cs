using Avalonia;

namespace MultiConverter.Extension;

public static class ResourceExtensions
{
    public static T? GetResource<T>(this string? key)
    {
        if(key is null)
        {
            return default;
        }

        if (Application.Current is not null &&
            Application.Current.Styles.TryGetResource(key, out object? style) &&
            style is T resource)
        {
            return resource;
        }

        return default;
    }
}
