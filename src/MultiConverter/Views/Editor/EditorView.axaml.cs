// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MultiConverter.Views.Editor;

public partial class EditorView : UserControl
{
    public EditorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

