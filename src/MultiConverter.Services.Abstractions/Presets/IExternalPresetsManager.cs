using System.Threading.Tasks;
using MultiConverter.Models.Presets;

namespace MultiConverter.Services.Abstractions.Presets;

public interface IExternalPresetsManager
{
    Task<Preset[]> GetPresetAsync(string presetsFilePath);

    Task<bool> TryExportPresetAsync(Preset[] presets, string presetsFilePath);
}
