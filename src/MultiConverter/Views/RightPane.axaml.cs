using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views;

public partial class RightPane : UserControl
{
    public RightPane() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
