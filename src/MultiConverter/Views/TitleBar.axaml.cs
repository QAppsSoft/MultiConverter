using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views;

public partial class TitleBar : UserControl
{
    public TitleBar() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
