using FFMpegCore;
using MultiConverter.Models.Configurations;
using MultiConverter.Models.Presets.Formats;
using MultiConverter.Services.Abstractions.Formats;

namespace MultiConverter.Services.Formats;

public class ContainersFormatProvider : IContainersFormatProvider
{
    private readonly FavoriteFormatsConfiguration _configuration;
    private readonly Lazy<IEnumerable<ContainerFormat>> _containers;

    public ContainersFormatProvider(FavoriteFormatsConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _configuration = configuration;

        _containers = new Lazy<IEnumerable<ContainerFormat>>(ContainersFactory);
    }

    public IEnumerable<ContainerFormat> Formats() => _containers.Value;

    private IEnumerable<ContainerFormat> ContainersFactory()
    {
        var containers = FFMpeg.GetContainerFormats();
        return containers.Where(f => f.MuxingSupported)
            .Select(format =>
            {
                bool favorite = _configuration.Favorites.Contains(format.Name);
                return new ContainerFormat(
                    format.Name,
                    format.Description,
                    format.Extension,
                    favorite
                );
            })
            .OrderByDescending(x => x.Favorite)
            .ThenBy(x => x.Name);
    }
}
