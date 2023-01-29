using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views;

public partial class ItemsView : UserControl
{
    public ItemsView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
