using System;
using AutoMapper;
using MultiConverter.Models.Presets.Enums;
using MultiConverter.Models.Presets.Formats;

namespace MultiConverter.DependencyInjection.AutoMapper;

public class CodecProfile : Profile
{
    public CodecProfile() =>
        CreateMap<FFMpegCore.Enums.Codec, MediaCodec>()
            .ConstructUsing(codec => new MediaCodec(
                codec.Name,
                MapType(codec.Type),
                codec.DecodingSupported,
                codec.EncodingSupported,
                codec.Description)
            );

    private static CodecType MapType(FFMpegCore.Enums.CodecType codecType) =>
        codecType switch
        {
            FFMpegCore.Enums.CodecType.Unknown => CodecType.Unknown,
            FFMpegCore.Enums.CodecType.Video => CodecType.Video,
            FFMpegCore.Enums.CodecType.Audio => CodecType.Audio,
            FFMpegCore.Enums.CodecType.Subtitle => CodecType.Subtitle,
            FFMpegCore.Enums.CodecType.Data => CodecType.Data,
            _ => throw new ArgumentOutOfRangeException(nameof(codecType), codecType, null)
        };
}
