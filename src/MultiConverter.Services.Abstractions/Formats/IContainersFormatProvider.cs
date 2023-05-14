using System.Collections.Generic;
using MultiConverter.Models.Presets.Formats;

namespace MultiConverter.Services.Abstractions.Formats;

public interface IContainersFormatProvider
{
    IEnumerable<ContainerFormat> Formats();
}
