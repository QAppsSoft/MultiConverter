using Autofac;
using MultiConverter.Common;
using MultiConverter.Infrastructure;
using MultiConverter.Services;
using MultiConverter.Services.Abstractions;
using MultiConverter.Services.Abstractions.Formats;
using MultiConverter.Services.Abstractions.Presets;
using MultiConverter.Services.Formats;
using MultiConverter.Services.Implementations;
using MultiConverter.Services.Presets;

namespace MultiConverter.DependencyInjection.Modules;

public class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SchedulerProvider>().As<ISchedulerProvider>();
        builder.RegisterType<LanguageManager>().As<ILanguageManager>().SingleInstance();
        builder.RegisterType<SystemSetterJob>().As<ISystemSetterJob>().SingleInstance();
        builder.RegisterType<DialogService>().As<IDialogService>();
        builder.RegisterType<CodecsProvider>().As<ICodecsProvider>().SingleInstance();
        builder.RegisterType<ContainersFormatProvider>().As<IContainersFormatProvider>().SingleInstance();
        builder.RegisterType<DefaultPresetsProvider>().As<IDefaultPresetsProvider>().SingleInstance();
    }
}
