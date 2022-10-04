using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MultiConverter.ViewModels.Options;
using ReactiveUI;

namespace MultiConverter.Views.Options;

public partial class OptionsView : ReactiveUserControl<OptionsViewModel>
{
    public OptionsView()
    {
        InitializeComponent();
        this.WhenActivated(_ => { });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

