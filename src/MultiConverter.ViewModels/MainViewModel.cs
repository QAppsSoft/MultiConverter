using System;
using System.Collections.Generic;
using System.Linq;
using MultiConverter.ViewModels.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel(IEnumerable<IPageViewModel> pages)
    {
        ArgumentNullException.ThrowIfNull(pages);

        Pages = pages;

        SelectedPage = Pages.First();
    }

    public IEnumerable<IPageViewModel> Pages { get; }

    [Reactive]
    public IPageViewModel SelectedPage { get; set; }
}
