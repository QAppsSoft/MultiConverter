using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Settings;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

