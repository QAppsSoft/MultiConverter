﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Presets.Options;

public partial class VideoSizeOptionView : UserControl
{
    public VideoSizeOptionView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}