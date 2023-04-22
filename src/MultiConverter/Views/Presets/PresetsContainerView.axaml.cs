using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MultiConverter.ViewModels.Presets;

namespace MultiConverter.Views.Presets;

public partial class PresetsContainerView : ReactiveUserControl<PresetsContainerViewModel>
{
    public PresetsContainerView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
