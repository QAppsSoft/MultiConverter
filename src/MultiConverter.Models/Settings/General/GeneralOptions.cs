using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using MultiConverter.Models.Settings.General.FileFilters;

namespace MultiConverter.Models.Settings.General;

public sealed record GeneralOptions([property: JsonConverter(typeof(JsonStringEnumConverter))]
    Theme Theme,
    string Language, int AnalysisTimeout, string TemporalFolder, string[] SupportedFilesExtensions,
    FileFilter[] FileFilters, bool LoadFilesAlreadyInQueue,
    bool CheckTemporalFolder, int CheckTemporalFolderEvery)
{
    private static readonly Lazy<GeneralOptions> s_defaultGeneralOptions = new(GenerateDefaultOption, true);

    public bool Equals(GeneralOptions? other)
    {
        if (other is null)
        {
            return false;
        }

        return Theme.Equals(other.Theme) &&
               Language.Equals(other.Language) &&
               AnalysisTimeout.Equals(other.AnalysisTimeout) &&
               LoadFilesAlreadyInQueue.Equals(other.LoadFilesAlreadyInQueue) &&
               TemporalFolder.Equals(other.TemporalFolder) &&
               SupportedFilesExtensions.OrderBy(x => x).SequenceEqual(other.SupportedFilesExtensions.OrderBy(x => x)) &&
               FileFilters.OrderBy(x => x).SequenceEqual(other.FileFilters.OrderBy(x => x));
    }

    private static GeneralOptions GenerateDefaultOption()
    {
        const Theme theme = Theme.Dark;
        const string language = "en";
        const int timeout = 60;
        const bool loadFilesAlreadyInQueue = false;
        const bool checkTemporalPath = true;
        const int checkTemporalPathEvery = 30;

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
            loadFilesAlreadyInQueue, checkTemporalPath, checkTemporalPathEvery);
    }

    public static GeneralOptions Default() => s_defaultGeneralOptions.Value;

    public override int GetHashCode()
    {
        int hashCode = 0;

        hashCode ^= Theme.GetHashCode();
        hashCode ^= Language.GetHashCode();
        hashCode ^= AnalysisTimeout.GetHashCode();
        hashCode ^= LoadFilesAlreadyInQueue.GetHashCode();
        hashCode ^= TemporalFolder.GetHashCode();
        hashCode ^= CheckTemporalFolder.GetHashCode();
        hashCode ^= CheckTemporalFolderEvery.GetHashCode();

        foreach (string item in SupportedFilesExtensions)
        {
            hashCode ^= item.GetHashCode();
        }

        foreach (FileFilter item in FileFilters)
        {
            hashCode ^= item.GetHashCode();
        }

        return hashCode;
    }
}
