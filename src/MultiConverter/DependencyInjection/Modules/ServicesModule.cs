using Autofac;
using MultiConverter.Common;
using MultiConverter.Infrastructure;
using MultiConverter.Services;
using MultiConverter.Services.Abstractions;
using MultiConverter.Services.Implementations;

namespace MultiConverter.DependencyInjection.Modules;

public class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SchedulerProvider>().As<ISchedulerProvider>();
        builder.RegisterType<LanguageManager>().As<ILanguageManager>().SingleInstance();
        builder.RegisterType<SystemSetterJob>().As<ISystemSetterJob>().SingleInstance();
        builder.RegisterType<DialogService>().As<IDialogService>();
    }
}
