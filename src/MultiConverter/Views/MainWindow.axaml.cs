using Avalonia;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Windowing;

namespace MultiConverter.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        TitleBar.ExtendsContentIntoTitleBar = true;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
