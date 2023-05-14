using AutoMapper;
using FFMpegCore;
using MultiConverter.Models.Presets.Formats;
using MultiConverter.Services.Abstractions.Formats;

namespace MultiConverter.Services.Formats;

public class ContainersFormatProvider : IContainersFormatProvider
{
    private readonly IMapper _mapper;
    private readonly Lazy<IEnumerable<ContainerFormat>> _containers;

    public ContainersFormatProvider(IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _containers = new Lazy<IEnumerable<ContainerFormat>>(ContainersFactory);
    }

    public IEnumerable<ContainerFormat> Formats() => _containers.Value;

    private IEnumerable<ContainerFormat> ContainersFactory()
    {
        var containers = FFMpeg.GetContainerFormats();
        return _mapper.Map<IEnumerable<FFMpegCore.Enums.ContainerFormat>, IEnumerable<ContainerFormat>>(containers);
    }
}
