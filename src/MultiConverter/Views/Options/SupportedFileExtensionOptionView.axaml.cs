using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Options;

public partial class SupportedFileExtensionOptionView : UserControl
{
    public SupportedFileExtensionOptionView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

