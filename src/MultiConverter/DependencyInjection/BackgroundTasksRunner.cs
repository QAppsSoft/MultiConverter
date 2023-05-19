using System.Threading.Tasks;
using FFMpegCore;
using MultiConverter.Models.Configurations;
using MultiConverter.Services.Abstractions.Formats;
using Splat;

namespace MultiConverter.DependencyInjection;

public static class BackgroundTasksRunner
{
    public static void Start(IReadonlyDependencyResolver resolver) =>
        Task.Run(() => RunTasksAsync(resolver));

    private static async Task RunTasksAsync(IReadonlyDependencyResolver resolver)
    {
        var overrides = resolver.GetRequiredService<ExtensionOverridesConfiguration>();
        var ffOptions = GlobalFFOptions.Current;
        ffOptions.ExtensionOverrides = overrides.Overrides;
        GlobalFFOptions.Configure(ffOptions);

        resolver.GetRequiredService<IContainersFormatProvider>().Formats();
        resolver.GetRequiredService<ICodecsProvider>().GetCodecs();
    }
}
