using System;
using MultiConverter.ViewModels;
using MultiConverter.ViewModels.Interfaces;
using MultiConverter.ViewModels.Presets;

namespace MultiConverter.Pages;

public sealed class PresetsContainerPage : PageViewModelBase<PresetsContainerViewModel>, IPresetsContainerPageViewModel
{
    public PresetsContainerPage(Func<PresetsContainerViewModel> factory) : base(factory)
    {
    }

    public override int Order => 2;
    public override string Name => "Presets";
    public override string Icon => PageIcons.PresetsIconKey;
}
