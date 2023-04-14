using System.Reactive.Linq;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Options;

public sealed class AudioSamplingRateOptionViewModel : OptionViewModelBase
{
    public AudioSamplingRateOptionViewModel(AudioSamplingRateOption audioSamplingRateOption, ISchedulerProvider schedulerProvider) : base(schedulerProvider)
    {
        SamplingRate = audioSamplingRateOption.SamplingRate;

        _ = this.WhenAnyValue(x => x.SamplingRate)
            .Select(samplingRate => samplingRate != audioSamplingRateOption.SamplingRate)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, x => x.HasChanged);

        InitializeDefaultOptions();
    }

    [Reactive] public int SamplingRate { get; set; }

    private void InitializeDefaultOptions() =>
        DefaultOptions = new[]
        {
            new ValuesUpdater { Caption = "44.1 kHz", Update = () => SamplingRate = 44100 },
            new ValuesUpdater { Caption = "48.0 kHz", Update = () => SamplingRate = 48000 },
            new ValuesUpdater { Caption = "96.0 kHz", Update = () => SamplingRate = 96000 },
        };

    public static implicit operator AudioSamplingRateOption(AudioSamplingRateOptionViewModel vm) => new(vm.SamplingRate);

    private static IOption ToOption(AudioSamplingRateOptionViewModel vm) => new AudioSamplingRateOption(vm.SamplingRate);

    public override IOption GetOption() => ToOption(this);
}
