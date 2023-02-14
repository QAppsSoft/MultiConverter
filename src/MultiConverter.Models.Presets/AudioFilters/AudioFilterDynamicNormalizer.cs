using FFMpegCore.Arguments;

namespace MultiConverter.Models.Presets.AudioFilters;

public record AudioFilterDynamicNormalizer(
    int FrameLength = 500,
    int FilterWindow = 31,
    double TargetPeak = 0.95,
    double GainFactor = 10.0,
    double TargetRms = 0.0,
    bool ChannelCoupling = true,
    bool EnableDcBiasCorrection = false,
    bool EnableAlternativeBoundary = false,
    double CompressorFactor = 0.0
) : AudioFilter
{
    public override IAudioFilterArgument GetFilter() =>
        new DynamicNormalizerArgument(
            FrameLength,
            FilterWindow,
            TargetPeak,
            GainFactor,
            TargetRms,
            ChannelCoupling,
            EnableDcBiasCorrection,
            EnableAlternativeBoundary,
            CompressorFactor
        );
}
