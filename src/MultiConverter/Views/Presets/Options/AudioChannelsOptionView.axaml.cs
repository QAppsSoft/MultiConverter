using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Presets.Options;

public partial class AudioChannelsOptionView : UserControl
{
    public AudioChannelsOptionView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
