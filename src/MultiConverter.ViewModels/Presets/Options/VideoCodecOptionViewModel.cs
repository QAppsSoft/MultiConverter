using System;
using System.Reactive.Linq;
using DynamicData.Binding;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Options;

public sealed class VideoCodecOptionViewModel : OptionViewModelBase
{
    public VideoCodecOptionViewModel(VideoCodecOption videoCodec, ISchedulerProvider schedulerProvider) :
        base(schedulerProvider)
    {
        Codec = videoCodec.Codec;

        _ = this.WhenAnyPropertyChanged()
            .WhereNotNull()
            .Select(x => videoCodec.Codec != x.Codec)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, x => x.HasChanged);

        InitializeDefaultOptions();
    }

    [Reactive] public string Codec { get; set; }

    private void InitializeDefaultOptions() => DefaultOptions = new[]
    {
        new ValuesUpdater { Caption = "MPG1", Update = () => Codec = "mpeg1video" },
        new ValuesUpdater { Caption = "MPG2", Update = () => Codec = "mpeg2video" },
        new ValuesUpdater { Caption = "MP4", Update = () => Codec = "mpeg4" },
        new ValuesUpdater { Caption = "AVI", Update = () => Codec = "libxvid" },
    };

    public static implicit operator VideoCodecOption(VideoCodecOptionViewModel vm) => new(vm.Codec);

    private static IOption ToOption(VideoCodecOptionViewModel vm) => new VideoCodecOption(vm.Codec);

    public override IOption GetOption() => ToOption(this);
}
