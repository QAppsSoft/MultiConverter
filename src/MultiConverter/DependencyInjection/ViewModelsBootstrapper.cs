using HanumanInstitute.MvvmDialogs;
using MultiConverter.Common;
using MultiConverter.Models.Settings.General;
using MultiConverter.Pages;
using MultiConverter.Services.Abstractions;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels;
using MultiConverter.ViewModels.Editor;
using MultiConverter.ViewModels.Interfaces;
using MultiConverter.ViewModels.Options;
using Splat;

namespace MultiConverter.DependencyInjection;

public static class ViewModelsBootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        RegisterCommonViewModels(services, resolver);
        RegisterPages(services, resolver);
    }

    private static void RegisterPages(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.Register<IGeneralPageViewModel>(() => new EditorPage(
            resolver.GetRequiredService<EditorViewModel>)
        );

        services.Register<IFooterPageViewModel>(() => new OptionsPage(
            resolver.GetRequiredService<OptionsViewModel>)
        );
    }

    private static void RegisterCommonViewModels(IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        services.Register(() => new MainViewModel(
            resolver.GetServices<IGeneralPageViewModel>(),
            resolver.GetServices<IFooterPageViewModel>())
        );

        services.Register(() => new EditorViewModel(
            resolver.GetRequiredService<ISchedulerProvider>())
        );

        services.Register(() => new OptionsViewModel(
            resolver.GetRequiredService<ISchedulerProvider>(),
            resolver.GetRequiredService<ISetting<GeneralOptions>>(),
            resolver.GetRequiredService<ILanguageManager>(),
            resolver.GetRequiredService<IDialogService>()
        ));
    }
}
