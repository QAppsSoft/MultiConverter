using FluentAvalonia.UI.Windowing;

namespace MultiConverter.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();

        TitleBar.ExtendsContentIntoTitleBar = true;
    }
}
