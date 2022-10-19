using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Controls;

public partial class FolderSelector : UserControl
{
    public FolderSelector()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

