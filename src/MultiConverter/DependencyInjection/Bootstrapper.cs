using Splat;

namespace MultiConverter.DependencyInjection;

public static class Bootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        ViewModelsBootstrapper.RegisterViewModels(services, resolver);
        ServicesBootstrapper.RegisterServices(services, resolver);
    }
}
