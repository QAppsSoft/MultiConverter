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
            resolver.GetRequiredService<LanguagesConfiguration>(),
            resolver.GetRequiredService<ISetting<GeneralOptions>>()
        ));
    }
}
