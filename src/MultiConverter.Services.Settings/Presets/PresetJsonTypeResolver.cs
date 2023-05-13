using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using MultiConverter.Models.Presets;
using MultiConverter.Models.Presets.AudioFilters;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using MultiConverter.Models.Presets.VideoFilters;

namespace MultiConverter.Services.Settings.Presets;

public class PresetJsonTypeResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        if (jsonTypeInfo.Type == typeof(IOption))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$option-type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(AudioBitrateOption), nameof(AudioBitrateOption)),
                    new JsonDerivedType(typeof(AudioCodecOption), nameof(AudioCodecOption)),
                    new JsonDerivedType(typeof(AudioSamplingRateOption), nameof(AudioSamplingRateOption)),

                    new JsonDerivedType(typeof(VideoAspectRatioOption), nameof(VideoAspectRatioOption)),
                    new JsonDerivedType(typeof(VideoBitrateOption), nameof(VideoBitrateOption)),
                    new JsonDerivedType(typeof(VideoCodecOption), nameof(VideoCodecOption)),
                    new JsonDerivedType(typeof(VideoFrameRateOption), nameof(VideoFrameRateOption)),
                    new JsonDerivedType(typeof(VideoSizeOption), nameof(VideoSizeOption)),
                }
            };
        }

        if (jsonTypeInfo.Type == typeof(VideoFilter))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$videofilter-type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(VideoFilterScale), nameof(VideoFilterScale)),
                }
            };
        }

        if (jsonTypeInfo.Type == typeof(AudioFilter))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$audiofilter-type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(AudioFilterDynamicNormalizer), nameof(AudioFilterDynamicNormalizer)),
                }
            };
        }

        return jsonTypeInfo;
    }
}
