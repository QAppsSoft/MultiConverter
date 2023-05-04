using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Presets.Options;

public partial class OptionsView : UserControl
{
    public OptionsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

