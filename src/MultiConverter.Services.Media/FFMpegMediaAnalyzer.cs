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

    private static IVideoStream CreateVideo(VideoInfo videoStream, string filePath) =>
        // ReSharper disable once HeapView.BoxingAllocation
        new VideoStream
        {
            Index = videoStream.Index,
            StreamIndex = 0,
            Source = filePath,
            AutoCropDimensions = Cropping.NoCrop(),
            Format = videoStream.CodecName,
            FrameRate = videoStream.FrameRate,
            Size = new Size(videoStream.Width, videoStream.Height)
        };

    private static IAudioStream CreateAudio(AudioInfo audioStream, int streamIndex, string filePath) =>
        // ReSharper disable once HeapView.BoxingAllocation
        new AudioStream
        {
            BitRate = audioStream.BitRate,
            Duration = audioStream.Duration,
            Format = audioStream.CodecName,
            IsDefault = GetDispositionValue(audioStream, "default"),
            IsForced = GetDispositionValue(audioStream, "forced"),
            Language = GetTagValue(audioStream, "title"),
            LanguageCode = GetTagValue(audioStream, "language"),
            SampleRate = audioStream.SampleRateHz,
            Index = audioStream.Index,
            StreamIndex = streamIndex,
            Source = filePath
        };

    private static ISubtitleStream CreateSubtitle(SubtitleInfo subtitleStream, int streamIndex, string filePath) =>
        // ReSharper disable once HeapView.BoxingAllocation
        new SubtitleStream
        {
            Index = subtitleStream.Index,
            StreamIndex = streamIndex,
            Source = filePath,
            IsDefault = GetDispositionValue(subtitleStream, "default"),
            IsForced = GetDispositionValue(subtitleStream, "forced"),
            Language = GetTagValue(subtitleStream, "title"),
            LanguageCode = GetTagValue(subtitleStream, "language"),
            SubtitleType = ParseSubtitleType(subtitleStream.CodecName),
            Title = string.Empty // TODO: Get title?
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
