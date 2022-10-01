using System;
using System.Collections.Generic;
using System.Linq;
using MultiConverter.ViewModels.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel(IEnumerable<IGeneralPageViewModel> pages, IEnumerable<IFooterPageViewModel> footerPages)
    {
        ArgumentNullException.ThrowIfNull(pages);
        ArgumentNullException.ThrowIfNull(footerPages);

        Pages = pages;

        FooterPages = footerPages;

        CurrentPage = Pages.First();
    }

    public IEnumerable<IPageViewModel> Pages { get; }

    public IEnumerable<IPageViewModel> FooterPages { get; }

    [Reactive]
    public IPageViewModel CurrentPage { get; set; }
}
