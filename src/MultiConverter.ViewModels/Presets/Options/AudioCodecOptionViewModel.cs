using System.Reactive.Linq;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Options;

public sealed class AudioCodecOptionViewModel : OptionViewModelBase
{
    public AudioCodecOptionViewModel(AudioCodecOption audioCodecOption, ISchedulerProvider schedulerProvider) : base(schedulerProvider)
    {
        AudioCodec = audioCodecOption.AudioCodec;

        _ = this.WhenAnyValue(x => x.AudioCodec)
            .Select(codec => codec != audioCodecOption.AudioCodec)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, x => x.HasChanged);

        InitializeDefaultOptions();
    }

    [Reactive] public string AudioCodec { get; set; }

    private void InitializeDefaultOptions() =>
        DefaultOptions = new[]
        {
            new ValuesUpdater { Caption = "MP1", Update = () => AudioCodec = "MP1" },
            new ValuesUpdater { Caption = "MP2", Update = () => AudioCodec = "MP2" },
            new ValuesUpdater { Caption = "MP3", Update = () => AudioCodec = "MP3" },
            new ValuesUpdater { Caption = "AAC", Update = () => AudioCodec = "AAC" }
        };

    public static implicit operator AudioCodecOption(AudioCodecOptionViewModel vm) => new(vm.AudioCodec);

    private static IOption ToOption(AudioCodecOptionViewModel vm) => new AudioCodecOption(vm.AudioCodec);

    public override IOption GetOption() => ToOption(this);
}
