using System;
using System.Collections.Generic;
using System.Linq;
using MultiConverter.ViewModels.Interfaces;

namespace MultiConverter.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel(IEnumerable<IPageViewModel> pages)
    {
        ArgumentNullException.ThrowIfNull(pages);

        Pages = pages;

        CurrentPage = Pages.First();
    }

    public IEnumerable<IPageViewModel> Pages { get; }

    public IPageViewModel? CurrentPage { get; set; }
}
