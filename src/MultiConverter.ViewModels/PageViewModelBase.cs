using System;
using MultiConverter.ViewModels.Interfaces;

namespace MultiConverter.ViewModels;

public abstract class PageViewModelBase<TViewModel> : ViewModelBase, IPageViewModel
where TViewModel : IViewModel
{
    private readonly Lazy<TViewModel> _lazyViewModel;

    protected PageViewModelBase(Func<TViewModel> factory)
    {
        _lazyViewModel = new Lazy<TViewModel>(factory);
    }

    public abstract int Order { get; }
    public abstract string Name { get; }
    public abstract string Icon { get; }

    public IViewModel ContentViewModel => _lazyViewModel.Value;
}
