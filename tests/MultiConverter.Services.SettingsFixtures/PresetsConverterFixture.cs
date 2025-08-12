using System.Text.Json;
using AwesomeAssertions;
using MultiConverter.Models.Presets;
using MultiConverter.Models.Presets.AudioFilters;
using MultiConverter.Models.Presets.Builder;
using MultiConverter.Models.Presets.Options;
using MultiConverter.Models.Presets.VideoFilters;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.Services.Settings.Presets;

namespace MultiConverter.Services.SettingsFixtures;

public class PresetsConverterFixture
{
    private readonly Preset[] _presets;
    private static JsonSerializerOptions SerializerOptions { get; } =
        new() { WriteIndented = true, TypeInfoResolver = new PresetJsonTypeResolver(), };

    public PresetsConverterFixture()
    {
        _presets = new[]
        {
            PresetBuilder.Configure()
                .AddOptions(options =>
                    options
                        .With(new AudioCodecOption("aac"))
                        .With(new AudioBitrateOption())
                        .With(new AudioSamplingRateOption())
                        .With(new VideoCodecOption("libxvid"))
                        .With(new VideoFrameRateOption(29.97d))
                )
                .AddVideoFilter(filter =>
                    filter
                        .With(new VideoFilterScale(400, 300))
                )
                .AddAudioFilter(filter =>
                    filter
                        .With(new AudioFilterDynamicNormalizer())
                )
                .Build(),

            PresetBuilder.Configure()
                .AddOptions(options =>
                    options
                        .With(new AudioCodecOption("aac"))
                        .With(new AudioBitrateOption())
                        .With(new AudioSamplingRateOption())
                )
                .AddAudioFilter(filter =>
                    filter
                        .With(new AudioFilterDynamicNormalizer())
                )
                .Build()
        };
    }

    [Test]
    public void Get_default_Presets()
    {
        PresetsConverter converter = new();

        Preset[] defaultValue = converter.GetDefaultValue();

        defaultValue.Should().BeEmpty();
    }


    [Test]
    public void Convert_to_Presets()
    {
        PresetsConverter converter = new();

        State state = new() { Version = 1, Value = JsonSerializer.Serialize(_presets, SerializerOptions) };

        Preset[] result = converter.Convert(state);

        result.Should()
            .BeEquivalentTo(_presets,
                o => o.ComparingByMembers<Preset>());
    }

    [Test]
    public void Convert_to_State()
    {
        PresetsConverter converter = new();

        State stateResult = converter.Convert(_presets);
        Preset[]? result = JsonSerializer.Deserialize<Preset[]>(stateResult.Value, SerializerOptions);

        stateResult.Version.Should().Be(1);
        result.Should()
            .BeEquivalentTo(_presets, o =>
                o.ComparingByMembers<Preset>());
    }

    [Test]
    public void Convert_empty_State()
    {
        PresetsConverter converter = new();

        Preset[] result = converter.Convert(State.Empty);

        result.Should().BeEquivalentTo(Array.Empty<Preset>());
    }


    [Test]
    public void Convert_with_not_supported_version_number()
    {
        PresetsConverter converter = new();

        State state = new() { Version = 999999, Value = JsonSerializer.Serialize(_presets, SerializerOptions) };

        Preset[] result = converter.Convert(state);

        result.Should().BeEquivalentTo(Array.Empty<Preset>());
    }
}
