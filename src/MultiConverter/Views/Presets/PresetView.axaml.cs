using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MultiConverter.ViewModels.Presets;

namespace MultiConverter.Views.Presets;

public partial class PresetView : ReactiveUserControl<PresetViewModel>
{
    public PresetView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

