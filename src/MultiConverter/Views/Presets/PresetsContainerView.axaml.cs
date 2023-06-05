using Avalonia.ReactiveUI;
using MultiConverter.ViewModels.Presets;

namespace MultiConverter.Views.Presets;

public partial class PresetsContainerView : ReactiveUserControl<PresetsContainerViewModel>
{
    public PresetsContainerView() => InitializeComponent();
}
