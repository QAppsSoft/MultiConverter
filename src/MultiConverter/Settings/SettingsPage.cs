using System;
using MultiConverter.ViewModels;
using MultiConverter.ViewModels.Interfaces;
using MultiConverter.ViewModels.Settings;

namespace MultiConverter.Settings;

public class SettingsPage : PageViewModelBase<SettingsViewModel>, IFooterPageViewModel
{
    public SettingsPage(Func<SettingsViewModel> factory) : base(factory)
    {
    }

    public override int Order => 1000;
    public override string Name => "Settings";
    public override string Icon => "Settings";
}
