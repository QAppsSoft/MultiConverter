using System;
using System.Reactive.Linq;
using DynamicData.Binding;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using MultiConverter.ViewModels.Presets.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Options;

public sealed class VideoFrameRateOptionViewModel : OptionViewModelBase, IOptionItem
{
    public VideoFrameRateOptionViewModel(VideoFrameRateOption frameRateOption, ISchedulerProvider schedulerProvider) :
        base(schedulerProvider)
    {
        FrameRate = frameRateOption.FrameRate;

        _ = this.WhenAnyPropertyChanged()
            .WhereNotNull()
            .Select(x => Math.Abs(frameRateOption.FrameRate - x.FrameRate) > double.Epsilon)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, x => x.HasChanged);

        InitializeDefaultOptions();
    }

    [Reactive] public double FrameRate { get; set; }

    [ObservableAsProperty] public bool HasChanged { get; }

    private void InitializeDefaultOptions() => DefaultOptions = new[]
    {
        new ValuesUpdater { Caption = "8 fps", Update = () => FrameRate = 8 },
        new ValuesUpdater { Caption = "12 fps", Update = () => FrameRate = 12 },
        new ValuesUpdater { Caption = "15 fps", Update = () => FrameRate = 15 },
        new ValuesUpdater { Caption = "23.976 fps", Update = () => FrameRate = 23.976 },
        new ValuesUpdater { Caption = "24 fps", Update = () => FrameRate = 24 },
        new ValuesUpdater { Caption = "25 fps", Update = () => FrameRate = 25 },
        new ValuesUpdater { Caption = "29.97 fps", Update = () => FrameRate = 29.97 },
        new ValuesUpdater { Caption = "30 fps", Update = () => FrameRate = 30 },
        new ValuesUpdater { Caption = "50 fps", Update = () => FrameRate = 50 },
        new ValuesUpdater { Caption = "59.94 fps", Update = () => FrameRate = 59.94 },
        new ValuesUpdater { Caption = "60 fps", Update = () => FrameRate = 60 },
        new ValuesUpdater { Caption = "120 fps", Update = () => FrameRate = 120 }
    };

    public static implicit operator VideoFrameRateOption(VideoFrameRateOptionViewModel vm) => new(vm.FrameRate);

    public static IOption ToOption(VideoFrameRateOptionViewModel vm) => new VideoFrameRateOption(vm.FrameRate);
}
