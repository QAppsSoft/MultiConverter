using FFMpegCore;
using MultiConverter.Models.Presets.Formats;
using MultiConverter.Services.Abstractions.Formats;

namespace MultiConverter.Services.Formats;

public class ContainersFormatProvider : IContainersFormatProvider
{
    private readonly Lazy<IEnumerable<ContainerFormat>> _containers;

    public ContainersFormatProvider()
    {
        _containers = new Lazy<IEnumerable<ContainerFormat>>(ContainersFactory);
    }

    public IEnumerable<ContainerFormat> Formats() => _containers.Value;

    private static IEnumerable<ContainerFormat> ContainersFactory()
    {
        var containers = FFMpeg.GetContainerFormats();
        return containers.Where(f => f.MuxingSupported)
            .Select(format =>
                new ContainerFormat(
                    format.Name,
                    format.Description,
                    format.Extension))
            .OrderBy(format => format.Extension);
    }
}
