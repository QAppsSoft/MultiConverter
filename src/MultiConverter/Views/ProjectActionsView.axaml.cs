using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views;

public partial class ProjectActionsView : UserControl
{
    public ProjectActionsView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
