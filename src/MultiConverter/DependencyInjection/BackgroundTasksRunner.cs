using System.Threading.Tasks;
using Splat;

namespace MultiConverter.DependencyInjection;

public static class BackgroundTasksRunner
{
    public static void Start(IReadonlyDependencyResolver resolver) =>
        Task.Run(() => RunTasksAsync(resolver));

    private static async Task RunTasksAsync(IReadonlyDependencyResolver resolver)
    {
        // In the future this should run backgrounds services
    }
}
