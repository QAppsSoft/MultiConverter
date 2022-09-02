using System;

namespace MultiConverter.Models.Media;

public readonly record struct AudioStream(double BitRate, TimeSpan Duration, string Format, bool IsDefault,
    bool IsForced, string Language, string LanguageCode, int SampleRate, int Index, int StreamIndex,
    string Source) : IAudioStream;
