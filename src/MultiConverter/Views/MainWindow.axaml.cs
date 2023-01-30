using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using FluentAvalonia.Styling;
using FluentAvalonia.UI.Media;
using FluentAvalonia.UI.Windowing;

namespace MultiConverter.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        TitleBar.ExtendsContentIntoTitleBar = true;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        var thm = AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>();
        thm.RequestedThemeChanged += OnRequestedThemeChanged;

        // Enable Mica on Windows 11
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && thm.RequestedTheme != FluentAvaloniaTheme.HighContrastModeString)
        {
            TransparencyBackgroundFallback = Brushes.Transparent;
            TransparencyLevelHint = WindowTransparencyLevel.Mica;

            TryEnableMicaEffect(thm);
        }
    }

    protected override void OnRequestedThemeChanged(FluentAvaloniaTheme sender, RequestedThemeChangedEventArgs args)
    {
        base.OnRequestedThemeChanged(sender, args);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (IsWindows11 && args.NewTheme != FluentAvaloniaTheme.HighContrastModeString)
            {
                TryEnableMicaEffect(sender);
            }
            else if (args.NewTheme == FluentAvaloniaTheme.HighContrastModeString)
            {
                // Clear the local value here, and let the normal styles take over for HighContrast theme
                SetValue(BackgroundProperty, AvaloniaProperty.UnsetValue);
            }
        }
    }

    private void TryEnableMicaEffect(FluentAvaloniaTheme thm)
    {
        if (thm.RequestedTheme == FluentAvaloniaTheme.DarkModeString)
        {
            var color = this.TryFindResource("SolidBackgroundFillColorBase", out var value)
                ? (Color2)(Color)value
                : new Color2(32, 32, 32);

            color = color.LightenPercent(-0.8f);

            Background = new ImmutableSolidColorBrush(color, 0.78);
        }
        else if (thm.RequestedTheme == FluentAvaloniaTheme.LightModeString)
        {
            // Similar effect here
            var color = this.TryFindResource("SolidBackgroundFillColorBase", out var value)
                ? (Color2)(Color)value
                : new Color2(243, 243, 243);

            color = color.LightenPercent(0.5f);

            Background = new ImmutableSolidColorBrush(color, 0.9);
        }
    }
}
