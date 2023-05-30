using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MultiConverter.Common;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Options;

public sealed class AudioChannelsOptionViewModel : OptionViewModelBase
{
    public AudioChannelsOptionViewModel(AudioChannelsOption audioChannelsOption, ISchedulerProvider schedulerProvider) : base(schedulerProvider)
    {
        SelectedChannelItem = ChannelItems.First(item => item.Channels == audioChannelsOption.Channels);

        _ = this.WhenAnyValue(x => x.SelectedChannelItem)
            .Select(channels => channels.Channels != audioChannelsOption.Channels)
            .ToPropertyEx(this, x => x.HasChanged);
    }

    [Reactive] public ChannelItem SelectedChannelItem { get; set; }

    public IEnumerable<ChannelItem> ChannelItems { get; set; } = new[]
    {
        new ChannelItem("Stereo", 2),
        new ChannelItem("Mono", 1)
    };

    public static implicit operator AudioChannelsOption(AudioChannelsOptionViewModel vm) => new(vm.SelectedChannelItem.Channels);

    private static IOption ToOption(AudioChannelsOptionViewModel vm) => new AudioChannelsOption(vm.SelectedChannelItem.Channels);

    public override IOption GetOption() => ToOption(this);
}

public record ChannelItem(string Name, int Channels);
