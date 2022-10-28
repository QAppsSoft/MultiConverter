namespace MultiConverter.ViewModels.Interfaces;

public interface IPageViewModel
{
    int Order { get; }
    string Name { get; }
    string Icon { get; }
    IViewModel ContentViewModel { get; }
}
