using AutoMapper;
using MultiConverter.Models.Presets.Formats;

namespace MultiConverter.DependencyInjection.AutoMapper;

public class ContainerFormatProfile : Profile
{
    public ContainerFormatProfile() =>
        CreateMap<FFMpegCore.Enums.ContainerFormat, ContainerFormat>()
            .ConstructUsing(format => new ContainerFormat(
                format.Name,
                format.Description,
                format.Extension
            ));
}
