using System.Reactive.Linq;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Options;

public class AudioBitrateOptionViewModel : OptionViewModelBase
{
    public AudioBitrateOptionViewModel(AudioBitrateOption audioBitrateOption, ISchedulerProvider schedulerProvider) :
        base(schedulerProvider)
    {
        Bitrate = audioBitrateOption.Bitrate;

        _ = this.WhenAnyValue(x => x.Bitrate)
            .Select(bitrate => bitrate != audioBitrateOption.Bitrate)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, x => x.HasChanged);

        InitializeDefaultOptions();
    }

    [Reactive] public int Bitrate { get; set; }

    private void InitializeDefaultOptions() =>
        DefaultOptions = new[]
        {
            new ValuesUpdater { Caption = "128 kbps", Update = () => Bitrate = 128 },
            new ValuesUpdater { Caption = "196 kbps", Update = () => Bitrate = 196 },
            new ValuesUpdater { Caption = "256 kbps", Update = () => Bitrate = 256 },
            new ValuesUpdater { Caption = "320 kbps", Update = () => Bitrate = 320 }
        };

    public static implicit operator AudioBitrateOption(AudioBitrateOptionViewModel vm) => new(vm.Bitrate);

    public static IOption ToOption(AudioBitrateOptionViewModel vm) => new AudioBitrateOption(vm.Bitrate);
}
