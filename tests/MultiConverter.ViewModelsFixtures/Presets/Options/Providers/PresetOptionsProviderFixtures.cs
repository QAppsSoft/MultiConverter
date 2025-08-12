using AwesomeAssertions;
using MultiConverter.ViewModels.Presets.Options.Providers;
using MultiConverter.ViewModels.Presets.Options.Providers.Interfaces;
using MultiConverter.ViewModels.Presets.Options.Providers.Strategy;
using MultiConverter.ViewModelsFixtures.Helper;

namespace MultiConverter.ViewModelsFixtures.Presets.Options.Providers;

[TestFixture]
public class PresetOptionsProviderFixtures
{
    [Test]
    public void Initialize_with_null_should_throw()
    {
        Action initialize = () => _ = new PresetOptionsProvider(null);

        initialize.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Generators_should_equal_number_of_options()
    {
        int optionSubclassesCount = OptionsHelper.GetOptionsSubclasses().Count();
        IPresetOptionsProvider fixture = InitializePresetOptionsProvider();

        IEnumerable<OptionGeneratorBase> results = fixture.Options;

        results.Count().Should().Be(optionSubclassesCount);
    }

    private static IPresetOptionsProvider InitializePresetOptionsProvider()
    {
        OptionGeneratorStrategy strategy = OptionGeneratorHelper.InitializeOptionGeneratorStrategy();

        return new PresetOptionsProvider(strategy);
    }
}
