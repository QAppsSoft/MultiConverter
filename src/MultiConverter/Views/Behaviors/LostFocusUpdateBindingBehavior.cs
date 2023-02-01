using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace MultiConverter.Views.Behaviors;

// Workaround to LostFocusUpdateBinding https://github.com/AvaloniaUI/Avalonia/issues/6071
public class LostFocusUpdateBindingBehavior : Behavior<TextBox>
{
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<LostFocusUpdateBindingBehavior, string>(
            "Text", defaultBindingMode: BindingMode.TwoWay);

    static LostFocusUpdateBindingBehavior() =>
        TextProperty.Changed.Subscribe(e => { ((LostFocusUpdateBindingBehavior)e.Sender).OnBindingValueChanged(); });

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    protected override void OnAttached()
    {
        AssociatedObject.LostFocus += OnLostFocus;
        base.OnAttached();
    }

    protected override void OnDetaching()
    {
        AssociatedObject.LostFocus -= OnLostFocus;
        base.OnDetaching();
    }

    private void OnLostFocus(object? sender, RoutedEventArgs e)
    {
        if (AssociatedObject != null)
        {
            Text = AssociatedObject.Text;
        }
    }

    private void OnBindingValueChanged()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.Text = Text;
        }
    }
}
