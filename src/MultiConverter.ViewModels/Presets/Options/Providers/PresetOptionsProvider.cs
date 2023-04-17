using System;
using System.Collections.Generic;
using System.Linq;
using MultiConverter.Models.Presets.Options;
using MultiConverter.ViewModels.Presets.Options.Providers.Interfaces;

namespace MultiConverter.ViewModels.Presets.Options.Providers;

public sealed class PresetOptionsProvider : IPresetOptionsProvider
{
    private readonly List<OptionGeneratorBase> _options;

    public PresetOptionsProvider(IOptionGeneratorStrategy optionGeneratorStrategy)
    {
        ArgumentNullException.ThrowIfNull(optionGeneratorStrategy);

        var types = new[]
        {
            typeof(AudioBitrateOption),
            typeof(AudioCodecOption),
            typeof(AudioSamplingRateOption),

            typeof(VideoFrameRateOption),
            typeof(VideoAspectRatioOption),
            typeof(VideoBitrateOption),
        };

        var options = types.Select(optionGeneratorStrategy.Generate);

        _options = new List<OptionGeneratorBase>(options);
    }

    public IEnumerable<OptionGeneratorBase> Options => _options;
}
