﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Settings.SupportedFileExtensions;

public partial class SupportedFileExtensionSettingItemView : UserControl
{
    public SupportedFileExtensionSettingItemView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
