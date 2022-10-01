using MultiConverter.Common;
using MultiConverter.Pages;
using MultiConverter.ViewModels;
using MultiConverter.ViewModels.Editor;
using MultiConverter.ViewModels.Interfaces;
using Splat;

namespace MultiConverter.DependencyInjection;

public static class ViewModelsBootstrapper
{
    public static void RegisterViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        RegisterCommonViewModels(services, resolver);
        RegisterPages(services, resolver);
    }

    private static void RegisterPages(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver) =>
        services.Register<IPageViewModel>(() => new EditorPage(resolver.GetRequiredService<EditorViewModel>));

    private static void RegisterCommonViewModels(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.Register(() => new MainViewModel(resolver.GetServices<IPageViewModel>()));

        services.Register(() => new EditorViewModel(resolver.GetRequiredService<ISchedulerProvider>()));
    }
}
