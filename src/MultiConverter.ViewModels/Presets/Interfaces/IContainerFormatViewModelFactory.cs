namespace MultiConverter.ViewModels.Presets.Interfaces;

public interface IContainerFormatViewModelFactory
{
    ContainerFormatViewModel Build(string containerFormat);
}
