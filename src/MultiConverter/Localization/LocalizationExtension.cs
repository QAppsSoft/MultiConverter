using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Avalonia;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Platform;

namespace MultiConverter.Localization;

public class LocalizeExtension : MarkupExtension
{
    public LocalizeExtension(string key)
    {
        this.Key = key;
    }

    public string Key { get; set; }

    public string Context { get; set; } = string.Empty;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var keyToUse = Key;
        if (!string.IsNullOrWhiteSpace(Context))
            keyToUse = $"{Context}/{Key}";

        var binding = new ReflectionBindingExtension($"[{keyToUse}]")
        {
            Mode = BindingMode.OneWay,
            Source = TranslationSource.Instance,
        };

        return binding.ProvideValue(serviceProvider);
    }
}
