using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using DynamicData.Binding;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using MultiConverter.Services.Abstractions.Formats;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Codec = MultiConverter.Models.Presets.Formats.Codec;

namespace MultiConverter.ViewModels.Presets.Options;

public sealed class VideoCodecOptionViewModel : OptionViewModelBase
{
    private readonly string _codec;

    public VideoCodecOptionViewModel(VideoCodecOption videoCodec, ICodecsProvider codecsProvider, ISchedulerProvider schedulerProvider) :
        base(schedulerProvider)
    {
        _codec = videoCodec.Codec;
        Codecs = codecsProvider.GetVideoCodecs();

        InitializeSelectedCodec();
        InitializeDefaultOptions();

        _ = this.WhenAnyPropertyChanged()
            .WhereNotNull()
            .Select(vm => videoCodec.Codec != vm.SelectedCodec.Name)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, vm => vm.HasChanged);
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

    private void InitializeDefaultOptions() => DefaultOptions = new[]
    {
        new ValuesUpdater { Caption = "MPEG-1", Update = () => SelectedCodec = SelectCodec("mpeg1video") },
        new ValuesUpdater { Caption = "MPEG-2", Update = () => SelectedCodec = SelectCodec("mpeg2video") },
        new ValuesUpdater { Caption = "MPEG-4", Update = () => SelectedCodec = SelectCodec("mpeg4") },
        new ValuesUpdater { Caption = "AVI (Xvid)", Update = () => SelectedCodec = SelectCodec("libxvid") },
    };

    public static implicit operator VideoCodecOption(VideoCodecOptionViewModel vm) => new(vm.SelectedCodec.Name);

    private static IOption ToOption(VideoCodecOptionViewModel vm) => new VideoCodecOption(vm.SelectedCodec.Name);

    public override IOption GetOption() => ToOption(this);
}
