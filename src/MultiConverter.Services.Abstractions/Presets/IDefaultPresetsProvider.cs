using System.Collections.Generic;
using MultiConverter.Models.PresetsProvider;

namespace MultiConverter.Services.Abstractions.Presets;

public interface IDefaultPresetsProvider
{
    IEnumerable<PresetsProviderItem> DefaultPresets { get; }
}
