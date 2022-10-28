using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Options;

public partial class VideoOptionView : UserControl
{
    public VideoOptionView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

