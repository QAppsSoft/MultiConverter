using MultiConverter.ViewModels;
using MultiConverter.ViewModels.Interfaces;

namespace MultiConverter.Pages;

public class EmptyPage : IPageViewModel
{
    public int Order => 0;
    public string Name => "Empty";
    public string Icon => PageIcons.SettingsIconKey;
    public IViewModel ContentViewModel => null!;
}
