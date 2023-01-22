using Autofac;
using MultiConverter.Pages;
using MultiConverter.ViewModels;
using MultiConverter.ViewModels.Editor;
using MultiConverter.ViewModels.Options;

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
        builder.RegisterType<OptionsPage>().AsImplementedInterfaces().SingleInstance();
    }

    private static void RegisterCommonViewModels(ContainerBuilder builder)
    {
        builder.RegisterType<MainViewModel>().ExternallyOwned();
        builder.RegisterType<EditorViewModel>().ExternallyOwned();
        builder.RegisterType<OptionsViewModel>().ExternallyOwned();

        builder.RegisterType<ThemeOptionItem>().ExternallyOwned();
        builder.RegisterType<LanguageOptionItem>().ExternallyOwned();
        builder.RegisterType<VideoOptionItem>().ExternallyOwned();
        builder.RegisterType<TemporalPathOptionItem>().ExternallyOwned();
        builder.RegisterType<SupportedFileExtensionOptionItem>().ExternallyOwned();
        builder.RegisterType<FileFiltersOptionItem>().ExternallyOwned();
    }
}
