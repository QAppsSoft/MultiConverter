using System.Reactive.Linq;
using DynamicData.Binding;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Options;

public sealed class VideoSizeOptionViewModel : OptionViewModelBase
{
    public VideoSizeOptionViewModel(VideoSizeOption videoSizeOption, ISchedulerProvider schedulerProvider) :
        base(schedulerProvider)
    {
        Width = videoSizeOption.VideoSize.Width;
        Height = videoSizeOption.VideoSize.Height;

        _ = this.WhenAnyPropertyChanged()
            .WhereNotNull()
            .Select(x =>
                videoSizeOption.VideoSize.Width != x.Width ||
                videoSizeOption.VideoSize.Height != x.Height)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, x => x.HasChanged);

        InitializeDefaultOptions();
    }

    [Reactive] public int Width { get; set; }

    [Reactive] public int Height { get; set; }

    private void InitializeDefaultOptions() => DefaultOptions = new[]
    {
        new ValuesUpdater { Caption = "320x240", Update = () => { Width = 320; Height = 240; } },
        new ValuesUpdater { Caption = "352x240", Update = () => { Width = 352; Height = 240; } },
        new ValuesUpdater { Caption = "400x300", Update = () => { Width = 400; Height = 300; } },
        new ValuesUpdater { Caption = "640x480", Update = () => { Width = 640; Height = 480; } },
        new ValuesUpdater { Caption = "720x480", Update = () => { Width = 720; Height = 480; } },
        new ValuesUpdater { Caption = "800x600", Update = () => { Width = 800; Height = 600; } },
        new ValuesUpdater { Caption = "1024x768", Update = () => { Width = 1024; Height = 768; } },
        new ValuesUpdater { Caption = "1600x1200", Update = () => { Width = 1600; Height = 1200; } },
        new ValuesUpdater { Caption = "1920x1080", Update = () => { Width = 1920; Height = 1080; } },
    };

    public static implicit operator VideoSizeOption(VideoSizeOptionViewModel vm) => new(new VideoSize(vm.Width, vm.Height));
    private static IOption ToOption(VideoSizeOptionViewModel vm) => new VideoSizeOption(new VideoSize(vm.Width, vm.Height));

    public override IOption GetOption() => ToOption(this);
}
