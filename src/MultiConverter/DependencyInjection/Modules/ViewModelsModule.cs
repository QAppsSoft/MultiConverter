using Autofac;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Pages;
using MultiConverter.ViewModels;
using MultiConverter.ViewModels.Editor;
using MultiConverter.ViewModels.Presets;
using MultiConverter.ViewModels.Presets.Factories;
using MultiConverter.ViewModels.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Options.Providers;
using MultiConverter.ViewModels.Presets.Options.Providers.Interfaces;
using MultiConverter.ViewModels.Presets.Options.Providers.Strategy;
using MultiConverter.ViewModels.Settings;

namespace MultiConverter.DependencyInjection.Modules;

public class ViewModelsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterCommonViewModels(builder);
        RegisterPages(builder);
        RegisterSettingsViewModel(builder);
        RegisterPresetsViewModel(builder);
    }

    private static void RegisterPages(ContainerBuilder builder)
    {
        builder.RegisterType<EditorPage>().AsImplementedInterfaces().SingleInstance();
        builder.RegisterType<PresetsContainerPage>().AsImplementedInterfaces().SingleInstance();
        builder.RegisterType<SettingsPage>().AsImplementedInterfaces().SingleInstance();
    }

    private static void RegisterCommonViewModels(ContainerBuilder builder)
    {
        builder.RegisterType<MainViewModel>().ExternallyOwned();
        builder.RegisterType<EditorViewModel>().ExternallyOwned();
    }

    private static void RegisterPresetsViewModel(ContainerBuilder builder)
    {
        builder.RegisterType<PresetsContainerViewModel>().ExternallyOwned();
        builder.RegisterType<PresetViewModel>().ExternallyOwned();
        builder.RegisterType<PresetViewModelFactory>().As<IPresetViewModelFactory>().ExternallyOwned();
        builder.RegisterType<ContainerFormatViewModelFactory>().As<IContainerFormatViewModelFactory>().ExternallyOwned();
        builder.RegisterType<OptionsViewModelFactory>().As<IOptionsViewModelFactory>().ExternallyOwned();
        builder.RegisterType<PresetOptionsProvider>().As<IPresetOptionsProvider>().ExternallyOwned();
        builder.RegisterType<OptionGeneratorStrategy>().As<IOptionGeneratorStrategy>().ExternallyOwned();

        // Register IOptionProcessor
        builder.RegisterType<AudioBitrateOptionProcessor>().As<IOptionProcessor<IOption>>().SingleInstance();
        builder.RegisterType<AudioCodecOptionProcessor>().As<IOptionProcessor<IOption>>().SingleInstance();
        builder.RegisterType<AudioSamplingRateOptionProcessor>().As<IOptionProcessor<IOption>>().SingleInstance();
        builder.RegisterType<VideoAspectRatioOptionProcessor>().As<IOptionProcessor<IOption>>().SingleInstance();
        builder.RegisterType<VideoBitrateOptionProcessor>().As<IOptionProcessor<IOption>>().SingleInstance();
        builder.RegisterType<VideoCodecOptionProcessor>().As<IOptionProcessor<IOption>>().SingleInstance();
        builder.RegisterType<VideoFrameRateOptionProcessor>().As<IOptionProcessor<IOption>>().SingleInstance();
        builder.RegisterType<VideoSizeOptionProcessor>().As<IOptionProcessor<IOption>>().SingleInstance();
    }

    private static void RegisterSettingsViewModel(ContainerBuilder builder)
    {
        builder.RegisterType<SettingsViewModel>().ExternallyOwned();
        builder.RegisterType<ThemeSettingItem>().AsImplementedInterfaces().ExternallyOwned();
        builder.RegisterType<LanguageSettingItem>().AsImplementedInterfaces().ExternallyOwned();
        builder.RegisterType<VideoSettingItem>().AsImplementedInterfaces().ExternallyOwned();
        builder.RegisterType<TemporalPathSettingItem>().AsImplementedInterfaces().ExternallyOwned();
        builder.RegisterType<FileFiltersSettingItem>().AsImplementedInterfaces().ExternallyOwned();
        builder.RegisterType<SupportedFileExtensionSettingItem>().AsImplementedInterfaces().ExternallyOwned();
    }
}
