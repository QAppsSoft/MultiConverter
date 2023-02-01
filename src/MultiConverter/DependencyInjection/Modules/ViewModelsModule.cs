using Autofac;
using MultiConverter.Settings;
using MultiConverter.ViewModels;
using MultiConverter.ViewModels.Editor;
using MultiConverter.ViewModels.Settings;

namespace MultiConverter.DependencyInjection.Modules;

public class ViewModelsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterCommonViewModels(builder);
        RegisterPages(builder);
    }

    private static void RegisterPages(ContainerBuilder builder)
    {
        builder.RegisterType<EditorPage>().AsImplementedInterfaces().SingleInstance();
        builder.RegisterType<SettingsPage>().AsImplementedInterfaces().SingleInstance();
    }

    private static void RegisterCommonViewModels(ContainerBuilder builder)
    {
        builder.RegisterType<MainViewModel>().ExternallyOwned();
        builder.RegisterType<EditorViewModel>().ExternallyOwned();
        builder.RegisterType<SettingsViewModel>().ExternallyOwned();

        builder.RegisterType<ThemeSettingItem>().AsImplementedInterfaces().ExternallyOwned();
        builder.RegisterType<LanguageSettingItem>().AsImplementedInterfaces().ExternallyOwned();
        builder.RegisterType<VideoSettingItem>().AsImplementedInterfaces().ExternallyOwned();
        builder.RegisterType<TemporalPathSettingItem>().AsImplementedInterfaces().ExternallyOwned();
        builder.RegisterType<FileFiltersSettingItem>().AsImplementedInterfaces().ExternallyOwned();
        builder.RegisterType<SupportedFileExtensionSettingItem>().AsImplementedInterfaces().ExternallyOwned();
    }
}
