using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using MultiConverter.Services.Abstractions.Settings;

namespace MultiConverter.Services.Settings;

public sealed class FileSettingsStore : ISettingsStore
{
    private readonly ILogger _logger;

    public FileSettingsStore(ILogger logger, string? path = null)
    {
        _logger = logger;
        Location = path ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "MultiConverter");

        var dir = new DirectoryInfo(Location);
        if (!dir.Exists) dir.Create();

        _logger.LogInformation("Settings folder is {Location}", Location);
    }

    private string Location { get; }

    public State Load(string key)
    {
        _logger.LogInformation("Reading setting for {Key}", key);

        var file = Path.Combine(Location, $"{key}.setting");
        var info = new FileInfo(file);

        if (!info.Exists || info.Length == 0) return State.Empty;

        var doc = XDocument.Load(file);
        var root = doc.ElementOrThrow(Structure.root);
        var versionString = root.AttributeOrThrow(Structure.version);
        var version = int.Parse(versionString);
        var state = root.ElementOrThrow(Structure.state);

        _logger.LogDebug("{Key} has the value {State}", key, state);
        return new State(version, state);
    }

    public void Save(string key, State state)
    {
        var file = Path.Combine(Location, $"{key}.setting");

        _logger.LogInformation("Creating setting for {Key}", key);

        var root = new XElement(new XElement(Structure.root, new XAttribute(Structure.version, state.Version)));
        root.Add(new XElement(Structure.state, state.Value));

        var doc = new XDocument(root);
        var fileText = doc.ToString();

        _logger.LogInformation("Writing settings for {Key} to {File}", key, file);
        File.WriteAllText(file, fileText);
        _logger.LogInformation("Setting  for {Key} committed", key);
    }

    private static class Structure
    {
        internal static readonly string root = "Setting";
        internal static readonly string state = "State";
        internal static readonly string version = "Version";
    }
}