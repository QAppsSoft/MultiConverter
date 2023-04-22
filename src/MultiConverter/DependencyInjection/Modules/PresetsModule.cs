using Autofac;
using MultiConverter.Models.Presets;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.Services.Settings.Presets;

namespace MultiConverter.DependencyInjection.Modules;

public class PresetsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterConverters(builder);
        RegisterPresets(builder);
    }

    private static void RegisterPresets(ContainerBuilder builder) =>
        builder.Register(context =>
        {
            var factory = context.Resolve<ISettingFactory>();
            IConverter<Preset[]> converter = context.Resolve<IConverter<Preset[]>>();
            return factory.Create(converter, "Presets");
        }).SingleInstance();

    private static void RegisterConverters(ContainerBuilder builder)
    {
        builder.RegisterType<PresetsConverter>()
            .As<IConverter<Preset[]>>()
            .SingleInstance();
    }
}
