using System.Text.Json;
using AwesomeAssertions;
using MultiConverter.Models.Settings;
using MultiConverter.Models.Settings.General;
using MultiConverter.Services.Abstractions.Settings;
using MultiConverter.Services.Settings.General;
using NUnit.Framework;

namespace MultiConverter.Services.SettingsFixtures;

public class GeneralOptionsConverterFixture
{
    [Test]
    public void Get_default_GeneralOptions()
    {
        GeneralOptionsConverter generalOptionsConverter = new();

        GeneralOptions defaultValue = generalOptionsConverter.GetDefaultValue();

        defaultValue.Should()
            .BeEquivalentTo(GeneralOptions.Default(),
                o => o.ComparingByMembers<GeneralOptions>());
    }

    [Test]
    public void Convert_to_GeneralOptions()
    {
        GeneralOptionsConverter generalOptionsConverter = new();
        GeneralOptions options = GeneralOptions.Default();

        State state = new() { Version = 1, Value = JsonSerializer.Serialize(options) };

        GeneralOptions generalOptionsResult = generalOptionsConverter.Convert(state);

        generalOptionsResult.Should()
            .BeEquivalentTo(options,
                o => o.ComparingByMembers<GeneralOptions>());
    }

    [Test]
    public void Convert_to_State()
    {
        GeneralOptionsConverter generalOptionsConverter = new();
        GeneralOptions options = GeneralOptions.Default();

        State stateResult = generalOptionsConverter.Convert(options);
        GeneralOptions generalOptionsResult = JsonSerializer.Deserialize<GeneralOptions>(stateResult.Value);

        stateResult.Version.Should().Be(1);
        generalOptionsResult.Should()
            .BeEquivalentTo(options, o =>
                o.ComparingByMembers<GeneralOptions>());
    }

    [Test]
    public void Convert_empty_State()
    {
        GeneralOptionsConverter generalOptionsConverter = new();

        GeneralOptions generalOptionsResult = generalOptionsConverter.Convert(State.Empty);

        generalOptionsResult.Should()
            .BeEquivalentTo(GeneralOptions.Default(),
                o => o.ComparingByMembers<GeneralOptions>());
    }

    // [Test]
    // public void Convert_to_uninitialized_GeneralOptions()
    // {
    //     GeneralOptionsConverter generalOptionsConverter = new();
    //     GeneralOptions options = new();
    //
    //     State state = new() { Version = 1, Value = JsonSerializer.Serialize(options) };
    //
    //     GeneralOptions generalOptionsResult = generalOptionsConverter.Convert(state);
    //
    //     generalOptionsResult.Should()
    //         .BeEquivalentTo(GeneralOptions.Default(),
    //             o => o.ComparingByMembers<GeneralOptions>());
    // }

    [Test]
    public void Convert_with_not_supported_version_number()
    {
        GeneralOptionsConverter generalOptionsConverter = new();
        GeneralOptions options = GeneralOptions.Default() with { Theme = Theme.Light };

        State state = new() { Version = 999999, Value = JsonSerializer.Serialize(options) };

        GeneralOptions generalOptionsResult = generalOptionsConverter.Convert(state);

        generalOptionsResult.Should()
            .BeEquivalentTo(GeneralOptions.Default(),
                o => o.ComparingByMembers<GeneralOptions>());
    }
}
