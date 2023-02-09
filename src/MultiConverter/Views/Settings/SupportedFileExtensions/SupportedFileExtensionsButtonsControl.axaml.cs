using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Settings.SupportedFileExtensions;

public partial class SupportedFileExtensionsButtonsControl : UserControl
{
    public SupportedFileExtensionsButtonsControl() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}


