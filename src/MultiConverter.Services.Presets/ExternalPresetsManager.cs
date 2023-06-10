using System.Text.Json;
using Microsoft.Extensions.Logging;
using MultiConverter.Models.Presets;
using MultiConverter.Services.Abstractions.Presets;
using MultiConverter.Services.Settings.Presets;

namespace MultiConverter.Services.Presets;

public class ExternalPresetsManager : IExternalPresetsManager
{
    private readonly ILogger<ExternalPresetsManager> _logger;

    private static JsonSerializerOptions SerializerOptions { get; } =
        new() { WriteIndented = true, TypeInfoResolver = new PresetJsonTypeResolver(), };

    public ExternalPresetsManager(ILogger<ExternalPresetsManager> logger)
    {
        _logger = logger;
    }

    public async Task<Preset[]> GetPresetAsync(string presetsFilePath)
    {
        Preset[]? presets = null;

        try
        {
            string jsonPreset = await File.ReadAllTextAsync(presetsFilePath);
            presets = JsonSerializer.Deserialize<Preset[]>(jsonPreset, SerializerOptions);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception processing {PresetPath}", presetsFilePath);
        }

        return presets ?? Array.Empty<Preset>();
    }

    public async Task<bool> TryExportPresetAsync(Preset[] presets, string presetsFilePath)
    {
        try
        {
            string jsonPreset = JsonSerializer.Serialize(presets, SerializerOptions);
            await File.WriteAllTextAsync(presetsFilePath, jsonPreset);
            return true;
        }
        catch (Exception exception)
        {
            var customProperties = new Dictionary<string, object> { { "Presets", presets } };
            using (_logger.BeginScope(customProperties))
            {
                _logger.LogError(exception, "Exception when saving preset to {Location} location", presetsFilePath);
            }

            return false;
        }
    }
}
