using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.ViewModels.Presets.Options.Providers;
using MultiConverter.ViewModels.Presets.Options.Providers.Interfaces;
using MultiConverter.ViewModels.Presets.Options.Providers.Strategy;

namespace MultiConverter.ViewModelsFixtures.Helper;

public static class OptionGeneratorHelper
{
    public static OptionGeneratorStrategy InitializeOptionGeneratorStrategy()
    {
        IOptionProcessor<IOption>[] operators =
        {
            new AudioBitrateOptionProcessor(),
            new AudioCodecOptionProcessor(),
            new AudioSamplingRateOptionProcessor(),

            new VideoFrameRateOptionProcessor(),
            new VideoAspectRatioOptionProcessor(),
            new VideoBitrateOptionProcessor(),
        };

        return new OptionGeneratorStrategy(operators);
    }
}
