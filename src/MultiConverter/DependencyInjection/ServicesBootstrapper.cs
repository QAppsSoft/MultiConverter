using MultiConverter.Common;
using MultiConverter.Infrastructure;
using Splat;

namespace MultiConverter.DependencyInjection;

public static class ServicesBootstrapper
{
    public static void RegisterServices(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver) =>
        services.Register<ISchedulerProvider>(() => new SchedulerProvider());
}
