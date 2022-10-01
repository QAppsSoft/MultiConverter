using System;
using System.IO;
using System.Text.Json.Serialization;
using MultiConverter.Models.Settings.General.FileFilters;

namespace MultiConverter.Models.Settings.General;

public readonly record struct GeneralOptions([property: JsonConverter(typeof(JsonStringEnumConverter))] Theme Theme,
    string Language, int AnalysisTimeout, string TemporalFolder, string[] SupportedFilesExtensions,
    FileFilter[] FileFilters, bool LoadFilesAlreadyInQueue)
{
    private static readonly Lazy<GeneralOptions> s_defaultGeneralOptions = new(GenerateDefaultOption, true);

    private static GeneralOptions GenerateDefaultOption()
    {
        const Theme theme = Theme.Dark;
        const string language = "en";
        const int timeout = 60;
        const bool loadFilesAlreadyInQueue = false;

        string temporalFolder = Path.GetTempPath();
        FileFilter[] fileFilters = Array.Empty<FileFilter>();

        string[] supportedFilesExtensions =
        {
            ".264", ".3g2", ".3gp", ".amv", ".asf", ".avi", ".avs", ".bik", ".cpk", ".dat", ".dif", ".divx", ".dv",
            ".dvr-ms", ".f4v", ".fli", ".flv", ".h264", ".m2ts", ".m4v", ".mkv", ".mkv", ".mod", ".mov", ".mp4",
            ".mp4", ".mpeg", ".mpg", ".mts", ".ogm", ".qt", ".rm", ".rmvb", ".ts", ".vfw", ".vob", ".webm", ".wmv",
            ".xv", ".yuv"
        };

        return new GeneralOptions(theme, language, timeout, temporalFolder, supportedFilesExtensions, fileFilters,
            loadFilesAlreadyInQueue);
    }

    public static GeneralOptions Default() => s_defaultGeneralOptions.Value;
}
