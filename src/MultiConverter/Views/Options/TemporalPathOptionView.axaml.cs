using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Options;

public partial class TemporalPathOptionView : UserControl
{
    public TemporalPathOptionView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

