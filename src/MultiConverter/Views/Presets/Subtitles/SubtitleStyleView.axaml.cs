using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Presets.Subtitles;

public partial class SubtitleStyleView : UserControl
{
    public SubtitleStyleView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

