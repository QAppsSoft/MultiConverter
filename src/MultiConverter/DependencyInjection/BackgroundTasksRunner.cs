using System.Threading.Tasks;
using MultiConverter.Services.Abstractions.Formats;
using Splat;

namespace MultiConverter.DependencyInjection;

public static class BackgroundTasksRunner
{
    public static void Start(IReadonlyDependencyResolver resolver) =>
        Task.Run(() => RunTasksAsync(resolver));

    private static async Task RunTasksAsync(IReadonlyDependencyResolver resolver)
    {
        resolver.GetRequiredService<IContainersFormatProvider>().Formats(); // Initialize in background
        resolver.GetRequiredService<ICodecsProvider>().GetCodecs(); // Initialize in background
    }
}
