using MultiConverter.Common;
using MultiConverter.Infrastructure;
using Splat;

namespace MultiConverter.DependencyInjection;

public static class ServicesBootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver) =>
        services.Register<ISchedulerProvider>(() => new SchedulerProvider());
}
