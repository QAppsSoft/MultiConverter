using Avalonia.ReactiveUI;
using MultiConverter.ViewModels.Settings;

namespace MultiConverter.Views.Settings;

public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
{
    public SettingsView() => InitializeComponent();
}
