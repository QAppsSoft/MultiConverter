namespace MultiConverter.Models.Media;

public readonly record struct SubtitleStream(int Index, int StreamIndex, string Source, bool IsDefault, bool IsForced,
    string Language, string LanguageCode, SubtitleType SubtitleType, string Title) : ISubtitleStream
{
    public bool CanBurnIn => SubtitleHelper.CheckIfCanBurnIn(SubtitleType);
public bool IsExternalSubtitle => SubtitleHelper.IsExternal(Source);
}
