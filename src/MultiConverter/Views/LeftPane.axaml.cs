using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views;

public partial class LeftPane : UserControl
{
    public LeftPane() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
