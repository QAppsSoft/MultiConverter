using System;
using MultiConverter.ViewModels;
using MultiConverter.ViewModels.Interfaces;
using MultiConverter.ViewModels.Options;

namespace MultiConverter.Pages;

public class OptionsPage : PageViewModelBase<OptionsViewModel>, IFooterPageViewModel
{
    public OptionsPage(Func<OptionsViewModel> factory) : base(factory)
    {
    }

    public override int Order => 1000;
    public override string Name => "Options";
    public override string Icon => "Settings";
}
