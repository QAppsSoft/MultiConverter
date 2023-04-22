using Autofac;
using MultiConverter.Models.Presets;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.Services.Settings;
using MultiConverter.Services.Settings.General;
using MultiConverter.Services.Settings.Presets;

namespace MultiConverter.DependencyInjection.Modules;

public class SettingsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterCommonInterfaces(builder);
        RegisterConverters(builder);
        RegisterSettings(builder);
        RegisterPresets(builder);
    }

    private static void RegisterSettings(ContainerBuilder builder) =>
        builder.Register(context =>
        {
            var factory = context.Resolve<ISettingFactory>();
            IConverter<GeneralOptions> converter = context.Resolve<IConverter<GeneralOptions>>();
            return factory.Create(converter, "GeneralOptions");
        }).SingleInstance();

    private static void RegisterPresets(ContainerBuilder builder) =>
        builder.Register(context =>
        {
            var factory = context.Resolve<ISettingFactory>();
            IConverter<Preset[]> converter = context.Resolve<IConverter<Preset[]>>();
            return factory.Create(converter, "Presets");
        }).SingleInstance();

    private static void RegisterConverters(ContainerBuilder builder)
    {
        builder.RegisterType<GeneralOptionsConverter>()
            .As<IConverter<GeneralOptions>>()
            .SingleInstance();

        builder.RegisterType<PresetsConverter>()
            .As<IConverter<Preset[]>>()
            .SingleInstance();
    }

    private static void RegisterCommonInterfaces(ContainerBuilder builder)
    {
        builder.RegisterType<SettingFactory>().As<ISettingFactory>().SingleInstance();
        builder.RegisterType<FileSettingsStore>().As<ISettingsStore>().SingleInstance();
    }
}
