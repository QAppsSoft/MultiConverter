using FluentAssertions;
using Moq.AutoMock;
using MultiConverter.Models.Settings.General;
using MultiConverter.ViewModels.Options;

namespace MultiConverter.ViewModelsFixtures.Options;

public class ThemeOptionItemTests : OptionsTestBase
{
    [Test]
    public void ThemeOptionItem_after_initialization()
    {
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using ThemeOptionItem fixture = mocker.CreateInstance<ThemeOptionItem>();

        fixture.Themes.Count().Should().Be(2);
        fixture.SelectedTheme.Should().Be(Theme.Dark);
        fixture.HasChanged.Should().BeFalse();
    }

    [Test]
    public void ThemeOptionItem_when_changed_HasChanged_should_be_true()
    {
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using ThemeOptionItem fixture = mocker.CreateInstance<ThemeOptionItem>();

        fixture.SelectedTheme = Theme.Light;

        fixture.SelectedTheme.Should().Be(Theme.Light);
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void ThemeOptionItem_when_changed_UpdateOption_should_change_theme()
    {
        AutoMocker mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using ThemeOptionItem fixture = mocker.CreateInstance<ThemeOptionItem>();

        fixture.SelectedTheme = Theme.Light;
        GeneralOptions option = fixture.UpdateOption(GeneralOptions.Default());

        fixture.SelectedTheme.Should().Be(Theme.Light);
        fixture.HasChanged.Should().BeTrue();
        option.Theme.Should().Be(Theme.Light);
    }
}
