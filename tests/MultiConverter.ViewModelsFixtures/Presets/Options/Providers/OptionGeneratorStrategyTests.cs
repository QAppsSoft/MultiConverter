using FluentAssertions;
using MultiConverter.Models.Presets.Options;
using MultiConverter.ViewModels.Presets.Options.Providers;
using MultiConverter.ViewModels.Presets.Options.Providers.Strategy;
using MultiConverter.ViewModelsFixtures.Helper;

namespace MultiConverter.ViewModelsFixtures.Presets.Options.Providers;

[TestFixture]
public class OptionGeneratorStrategyTests
{
    [Test]
    public void Should_throw_on_null_constructor_parameter()
    {
        Action initialization = () => _ = new OptionGeneratorStrategy(null);

        initialization.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Passing_a_wrong_type_should_throw()
    {
        OptionGeneratorStrategy fixture = OptionGeneratorHelper.InitializeOptionGeneratorStrategy();

        Action generate = () => fixture.Generate(typeof(int));

        generate.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void Passing_null_should_throw()
    {
        OptionGeneratorStrategy fixture = OptionGeneratorHelper.InitializeOptionGeneratorStrategy();

        Action generate = () => fixture.Generate(null);

        generate.Should().Throw<InvalidOperationException>();
    }

    [Test]
    [TestCase(typeof(AudioBitrateOption), typeof(AudioBitrateOptionGenerator))]
    [TestCase(typeof(AudioCodecOption), typeof(AudioCodecOptionGenerator))]
    [TestCase(typeof(VideoFrameRateOption), typeof(VideoFrameRateOptionGenerator))]
    public void Should_generate_correct_type(Type optionType, Type generatedType)
    {
        OptionGeneratorStrategy fixture = OptionGeneratorHelper.InitializeOptionGeneratorStrategy();

        var result = fixture.Generate(optionType);

        result.GetType().Should().Be(generatedType);
    }
}
