using System.Reactive.Linq;
using DynamicData.Binding;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Options;

public sealed class VideoAspectRatioOptionViewModel : OptionViewModelBase
{
    public VideoAspectRatioOptionViewModel(VideoAspectRatioOption aspectRatioOption, ISchedulerProvider schedulerProvider) :
        base(schedulerProvider)
    {
        AspectRatio = aspectRatioOption.AspectRatio;

        _ = this.WhenAnyPropertyChanged()
            .WhereNotNull()
            .Select(x => aspectRatioOption.AspectRatio != x.AspectRatio)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, x => x.HasChanged);

        InitializeDefaultOptions();
    }

    [Reactive] public string AspectRatio { get; set; }

    private void InitializeDefaultOptions() => DefaultOptions = new[]
    {
        new ValuesUpdater { Caption = "4:3", Update = () => AspectRatio = "4:3" },
        new ValuesUpdater { Caption = "16:9", Update = () => AspectRatio = "16:9" },
    };

    public static implicit operator VideoAspectRatioOption(VideoAspectRatioOptionViewModel vm) => new(vm.AspectRatio);

    private static IOption ToOption(VideoAspectRatioOptionViewModel vm) => new VideoAspectRatioOption(vm.AspectRatio);

    public override IOption GetOption() => ToOption(this);
}
