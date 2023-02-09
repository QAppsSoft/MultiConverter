using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Settings.SupportedFileExtensions;

public partial class SupportedFileExtensionsContentControl : UserControl
{
    public SupportedFileExtensionsContentControl() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}


