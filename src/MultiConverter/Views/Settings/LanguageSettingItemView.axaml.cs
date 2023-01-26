using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Settings;

public partial class LanguageSettingItemView : UserControl
{
    public LanguageSettingItemView() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}


