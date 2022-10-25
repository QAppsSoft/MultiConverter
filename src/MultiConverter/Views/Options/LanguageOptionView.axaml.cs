using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Options;

public partial class LanguageOptionView : UserControl
{
    public LanguageOptionView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

