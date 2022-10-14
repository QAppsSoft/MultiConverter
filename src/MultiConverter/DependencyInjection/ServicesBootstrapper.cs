using Avalonia;
using FluentAvalonia.Styling;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using Microsoft.Extensions.Logging;
using MultiConverter.Common;
using MultiConverter.Configuration;
using MultiConverter.Infrastructure;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.Services.Implementations;
using Splat;

namespace MultiConverter.DependencyInjection;

public static class ServicesBootstrapper
{
    public static void Register(IMutableDependencyResolver services, IReadonlyDependencyResolver resolver)
    {
        services.Register<ISchedulerProvider>(() => new SchedulerProvider());

        services.RegisterLazySingleton<ILanguageManager>(() => new LanguageManager(
            resolver.GetRequiredService<LanguagesConfiguration>()
        ));

        services.RegisterLazySingleton<ISystemSetterJob>(() => new SystemSetterJob(
            resolver.GetRequiredService<ISchedulerProvider>(),
            resolver.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(SystemSetterJob)),
            resolver.GetRequiredService<ISetting<GeneralOptions>>(),
            resolver.GetRequiredService<ILanguageManager>(),
            AvaloniaLocator.Current.GetRequiredService<FluentAvaloniaTheme>()
        ));

        services.RegisterLazySingleton<IDialogService>(() => new DialogService(
            resolver.GetRequiredService<DialogManager>()
        ));

        services.RegisterLazySingleton<DialogManager>(() =>
                new DialogManager() // TODO: Register ViewLocator() for custom dialogs use
        );
    }
}
