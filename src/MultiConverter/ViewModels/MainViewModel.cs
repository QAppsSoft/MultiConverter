using System.Collections.Generic;
using System.Linq;
using MultiConverter.Pages;
using MultiConverter.ViewModels.Editor;
using MultiConverter.ViewModels.Interfaces;

namespace MultiConverter.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        Pages = new IPageViewModel[] { new EditorPage(() => new EditorViewModel()) };

        CurrentPage = Pages.First();
    }

    public IEnumerable<IPageViewModel> Pages { get; }

    public IPageViewModel? CurrentPage { get; set; }
}
