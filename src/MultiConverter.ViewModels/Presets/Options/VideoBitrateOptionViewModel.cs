using System.Reactive.Linq;
using DynamicData.Binding;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Options;

public sealed class VideoBitrateOptionViewModel : OptionViewModelBase
{
    public VideoBitrateOptionViewModel(VideoBitrateOption bitrate, ISchedulerProvider schedulerProvider) :
        base(schedulerProvider)
    {
        Bitrate = bitrate.Bitrate;

        _ = this.WhenAnyPropertyChanged()
            .WhereNotNull()
            .Select(x => bitrate.Bitrate != x.Bitrate)
            .ObserveOn(schedulerProvider.Dispatcher)
            .ToPropertyEx(this, x => x.HasChanged);
    }

    [Reactive] public int Bitrate { get; set; }

    public static implicit operator VideoBitrateOption(VideoBitrateOptionViewModel vm) => new(vm.Bitrate);

    private static IOption ToOption(VideoBitrateOptionViewModel vm) => new VideoBitrateOption(vm.Bitrate);

    public override IOption GetOption() => ToOption(this);
}
