using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Presets.Options;

public partial class AudioBitrateOptionView : UserControl
{
    public AudioBitrateOptionView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

