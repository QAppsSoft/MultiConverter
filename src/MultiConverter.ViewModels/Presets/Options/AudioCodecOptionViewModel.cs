using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Formats;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using MultiConverter.Services.Abstractions.Formats;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Options;

public sealed class AudioCodecOptionViewModel : OptionViewModelBase
{
    private readonly string _codec;

    public AudioCodecOptionViewModel(AudioCodecOption audioCodecOption, ICodecsProvider codecsProvider, ISchedulerProvider schedulerProvider) : base(schedulerProvider)
    {
        _codec = audioCodecOption.AudioCodec;
        Codecs = codecsProvider.GetAudioCodecs();

        InitializeSelectedCodec();
        InitializeDefaultOptions();

        _ = this.WhenAnyValue(x => x.SelectedCodec)
            .Select(codec => codec.Name != audioCodecOption.AudioCodec)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, x => x.HasChanged);
    }

    public IEnumerable<Codec> Codecs { get; }

    [Reactive] public Codec SelectedCodec { get; set; }

    private void InitializeSelectedCodec()
    {
        SelectedCodec = SelectCodec(_codec);
    }

    private Codec SelectCodec(string codecName)
    {
        return Codecs.First(codec => codec.Name == codecName);
    }

    private void InitializeDefaultOptions() =>
        DefaultOptions = new[]
        {
            new ValuesUpdater { Caption = "MP2", Update = () => SelectedCodec = SelectCodec("mp2") },
            new ValuesUpdater { Caption = "MP3", Update = () => SelectedCodec = SelectCodec("libmp3lame") },
            new ValuesUpdater { Caption = "AAC", Update = () => SelectedCodec = SelectCodec("aac") }
        };

    public static implicit operator AudioCodecOption(AudioCodecOptionViewModel vm) => new(vm.SelectedCodec.Name);

    private static IOption ToOption(AudioCodecOptionViewModel vm) => new AudioCodecOption(vm.SelectedCodec.Name);

    public override IOption GetOption() => ToOption(this);
}
