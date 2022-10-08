using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace MultiConverter.Controls;

public class OptionsExpanderDisplayItem : TemplatedControl
{
    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<OptionsExpanderDisplayItem, string>(nameof(Header));

    public static readonly StyledProperty<IControl> ActionButtonProperty =
        AvaloniaProperty.Register<OptionsExpanderDisplayItem, IControl>(nameof(ActionButton));

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public IControl ActionButton
    {
        get => GetValue(ActionButtonProperty);
        set => SetValue(ActionButtonProperty, value);
    }
}
