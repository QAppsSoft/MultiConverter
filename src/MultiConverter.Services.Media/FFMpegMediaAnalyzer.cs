using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FFMpegCore;
using MultiConverter.Models.Media;
using MultiConverter.Services.Abstractions.Media;
using AudioInfo = FFMpegCore.AudioStream;
using AudioStream = MultiConverter.Models.Media.AudioStream;
using VideoInfo = FFMpegCore.VideoStream;
using SubtitleInfo = FFMpegCore.SubtitleStream;
using SubtitleStream = MultiConverter.Models.Media.SubtitleStream;
using VideoStream = MultiConverter.Models.Media.VideoStream;


namespace MultiConverter.Services.Media;

public class FFMpegMediaAnalyzer : IMediaAnalyzer
{
    public Task<IContainer> AnalyzeAsync(string filePath, CancellationToken? cancellationToken = null) =>
        AnalyzeAsync(filePath, AnalyzerOptions.Default(), cancellationToken);

    public async Task<IContainer> AnalyzeAsync(string filePath, AnalyzerOptions options,
        CancellationToken? cancellationToken = null)
    {
        CancellationTokenSource cancellationTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(cancellationToken ?? CancellationToken.None);

        cancellationTokenSource.CancelAfter(options.Timeout);

        IMediaAnalysis mediaAnalysis = await FFProbe.AnalyseAsync(filePath, null, cancellationTokenSource.Token)
            .ConfigureAwait(false);

        return BuildContainer(mediaAnalysis, filePath);
    }

    private static IContainer BuildContainer(IMediaAnalysis mediaAnalysis, string filePath)
    {
        IVideoStream? video = mediaAnalysis.PrimaryVideoStream != null
            ? CreateVideo(mediaAnalysis.PrimaryVideoStream, filePath)
            : null;

        IEnumerable<IAudioStream> audioStreams = ProcessAudio(mediaAnalysis, filePath);

        IEnumerable<ISubtitleStream> subtitleStreams = ProcessSubtitles(mediaAnalysis, filePath);

        // ReSharper disable once HeapView.BoxingAllocation
        return new Container
        {
            AudioTracks = audioStreams,
            Duration = mediaAnalysis.Duration,
            Source = filePath,
            Subtitles = subtitleStreams,
            Video = video
        };
    }

    private static IEnumerable<IAudioStream> ProcessAudio(IMediaAnalysis mediaInfo, string filePath) =>
        mediaInfo.AudioStreams.Select((audioStream, index) =>
            CreateAudio(audioStream, index, filePath));

    private static IEnumerable<ISubtitleStream> ProcessSubtitles(IMediaAnalysis mediaInfo, string filePath) =>
        mediaInfo.SubtitleStreams.Select((subtitleStream, index) =>
            CreateSubtitle(subtitleStream, index, filePath));

    private static IVideoStream CreateVideo(VideoInfo videoInfo, string filePath) =>
        // ReSharper disable once HeapView.BoxingAllocation
        new VideoStream
        {
            Index = videoInfo.Index,
            StreamIndex = 0,
            Source = filePath,
            AutoCropDimensions = Cropping.NoCrop(),
            Format = videoInfo.CodecName,
            FrameRate = videoInfo.FrameRate,
            Size = new Size(videoInfo.Width, videoInfo.Height)
        };

    private static IAudioStream CreateAudio(AudioInfo audioInfo, int streamIndex, string filePath) =>
        // ReSharper disable once HeapView.BoxingAllocation
        new AudioStream
        {
            BitRate = audioInfo.BitRate,
            Duration = audioInfo.Duration,
            Format = audioInfo.CodecName,
            IsDefault = GetDispositionValue(audioInfo, "default"),
            IsForced = GetDispositionValue(audioInfo, "forced"),
            Language = string.Empty,
            LanguageCode = audioInfo.Language ?? string.Empty,
            SampleRate = audioInfo.SampleRateHz,
            Index = audioInfo.Index,
            StreamIndex = streamIndex,
            Source = filePath
        };

    private static ISubtitleStream CreateSubtitle(SubtitleInfo subtitleInfo, int streamIndex, string filePath) =>
        // ReSharper disable once HeapView.BoxingAllocation
        new SubtitleStream
        {
            Index = subtitleInfo.Index,
            StreamIndex = streamIndex,
            Source = filePath,
            IsDefault = GetDispositionValue(subtitleInfo, "default"),
            IsForced = GetDispositionValue(subtitleInfo, "forced"),
            Language = string.Empty,
            LanguageCode = subtitleInfo.Language ?? string.Empty,
            SubtitleType = ParseSubtitleType(subtitleInfo.CodecName),
            Title = GetTagValue(subtitleInfo, "title")
        };

    private static bool GetDispositionValue(MediaStream mediaStream, string key) =>
        mediaStream.Disposition != null &&
        mediaStream.Disposition.TryGetValue(key, out bool dispositionValue) &&
        dispositionValue;

    private static string GetTagValue(MediaStream mediaStream, string key) =>
        mediaStream.Tags != null && mediaStream.Tags.TryGetValue(key, out string? tagValue)
            ? tagValue
            : string.Empty;

    private static SubtitleType ParseSubtitleType(string codecName) =>
        Enum.TryParse(codecName, true, out SubtitleType subtitleType) ? subtitleType : SubtitleType.Unknown;
}
