using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Controls;

public partial class WipPanel : UserControl
{
    public WipPanel()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

