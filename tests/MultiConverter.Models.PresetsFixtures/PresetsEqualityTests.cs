using MultiConverter.Models.Presets;
using MultiConverter.Models.Presets.AudioFilters;
using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;
using MultiConverter.Models.Presets.VideoFilters;

namespace MultiConverter.Models.PresetsFixtures;

[TestFixture]
public class PresetsEqualityTests
{
    [Test]
    public void Compare_two_empty_presets_should_be_equal()
    {
        Preset p1 = EmptyPreset();
        Preset p2 = EmptyPreset();

        bool equals = p1.Equals(p2);

        equals.Should().BeTrue();
    }

    [Test]
    public void Compare_two_equals_non_empty_presets_should_be_equal()
    {
        Preset p1 = FilledPreset();
        Preset p2 = FilledPreset();

        bool equals = p1.Equals(p2);

        equals.Should().BeTrue();
    }

    [Test]
    public void Compare_two_array_of_empty_presets_should_be_equal()
    {
        Preset[] a1 = { EmptyPreset(), FilledPreset() };
        Preset[] a2 = { EmptyPreset(), FilledPreset() };

        bool equals = a1.SequenceEqual(a2);

        equals.Should().BeTrue();
    }

    [Test]
    public void Compare_two_array_of_different_presets_should_not_be_equal()
    {
        Preset[] a1 = { FilledPreset(), EmptyPreset() };
        Preset[] a2 = { EmptyPreset(), FilledPreset() };

        bool equals = a1.SequenceEqual(a2);

        equals.Should().BeFalse();
    }

    [Test]
    public void Compare_two_array_of_slightly_different_presets_should_not_be_equal()
    {
        Preset[] a1 =
        {
            EmptyPreset(),
            EmptyPreset() with { VideoFilter = new VideoFilter[] { new VideoFilterScale(1024, 768) } }
        };
        Preset[] a2 = { EmptyPreset(), EmptyPreset() };

        bool equals = a1.SequenceEqual(a2);

        equals.Should().BeFalse();
    }

    private static Preset EmptyPreset() => Preset.Empty;

    private static Preset FilledPreset() => Preset.Empty with
    {
        VideoFilter = new VideoFilter[] { new VideoFilterScale(1024, 768) },
        AudioFilter = new AudioFilter[] { new AudioFilterDynamicNormalizer() },
        Options = new IOption[] { new AudioCodecOption("mpeg4") }
    };
}
