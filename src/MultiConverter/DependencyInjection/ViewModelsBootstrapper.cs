using MultiConverter.Common;
using MultiConverter.Models.Settings.General;
using MultiConverter.Pages;
using MultiConverter.Services;
using MultiConverter.Services.Abstractions;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.ViewModels;
using MultiConverter.ViewModels.Editor;
using MultiConverter.ViewModels.Interfaces;
using MultiConverter.ViewModels.Options;
using MultiConverter.ViewModels.Options.Interfaces;
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
        services.Register<IPageViewModel>(() => new EditorPage(
            resolver.GetRequiredService<EditorViewModel>)
        );

        services.Register<IPageViewModel>(() => new OptionsPage(
            resolver.GetRequiredService<OptionsViewModel>)
        );
    }

    private static void RegisterCommonViewModels(IMutableDependencyResolver services,
        IReadonlyDependencyResolver resolver)
    {
        services.Register(() => new MainViewModel(
            resolver.GetServices<IPageViewModel>()
        ));

        services.Register(() => new EditorViewModel(
            resolver.GetRequiredService<ISchedulerProvider>())
        );

        services.Register<IOptionItem>(() => new ThemeOptionItem(
            resolver.GetRequiredService<ISchedulerProvider>(),
            resolver.GetRequiredService<ISetting<GeneralOptions>>()
        ));

        services.Register<IOptionItem>(() => new LanguageOptionItem(
            resolver.GetRequiredService<ISchedulerProvider>(),
            resolver.GetRequiredService<ISetting<GeneralOptions>>(),
            resolver.GetRequiredService<ILanguageManager>()
        ));

        services.Register<IOptionItem>(() => new VideoOptionItem(
            resolver.GetRequiredService<ISchedulerProvider>(),
            resolver.GetRequiredService<ISetting<GeneralOptions>>()
        ));

        services.Register<IOptionItem>(() => new TemporalPathOptionItem(
            resolver.GetRequiredService<ISchedulerProvider>(),
            resolver.GetRequiredService<ISetting<GeneralOptions>>(),
            resolver.GetRequiredService<IDialogService>()
        ));

        services.Register<IOptionItem>(() => new SupportedFileExtensionOptionItem(
            resolver.GetRequiredService<ISchedulerProvider>(),
            resolver.GetRequiredService<ISetting<GeneralOptions>>()
        ));

        services.Register<IOptionItem>(() => new FileFiltersOptionItem(
            resolver.GetRequiredService<ISchedulerProvider>(),
            resolver.GetRequiredService<ISetting<GeneralOptions>>()
        ));

        services.Register(() => new OptionsViewModel(
            resolver.GetRequiredService<ISchedulerProvider>(),
            resolver.GetRequiredService<ISetting<GeneralOptions>>(),
            resolver.GetServices<IOptionItem>()
        ));
    }
}
