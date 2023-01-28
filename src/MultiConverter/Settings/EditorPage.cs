using System;
using MultiConverter.ViewModels;
using MultiConverter.ViewModels.Editor;
using MultiConverter.ViewModels.Interfaces;

namespace MultiConverter.Settings;

public class EditorPage : PageViewModelBase<EditorViewModel>, IGeneralPageViewModel
{
    public EditorPage(Func<EditorViewModel> factory) : base(factory)
    {
    }

    public override int Order => 1;
    public override string Name => "Editor";

    public override string Icon => PageIcons.EditorIconKey;
}
