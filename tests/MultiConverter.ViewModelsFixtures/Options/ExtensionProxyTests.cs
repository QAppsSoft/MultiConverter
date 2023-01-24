using FluentAssertions;
using MultiConverter.ViewModels.Settings;

namespace MultiConverter.ViewModelsFixtures.Options;

public class ExtensionProxyTests
{
    [Test]
    public void New_ExtensionProxy_should_HasChanged_true()
    {
        ExtensionProxy fixture = new();

        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void ExtensionProxy_with_empty_extension_should_HasChanged_true()
    {
        ExtensionProxy fixture = new(string.Empty);

        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void ExtensionProxy_with_non_empty_extension_should_HasChanged_false()
    {
        ExtensionProxy fixture = new(".avi");

        fixture.HasChanged.Should().BeFalse();
    }

    [Test]
    public void Changing_ExtensionProxy_Extension_should_HasChanged_true()
    {
        ExtensionProxy fixture = new(".avi");

        fixture.Extension = ".mkv";

        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void New_ExtensionProxy_with_empty_extension_should_Editing_true()
    {
        ExtensionProxy fixture = new();

        fixture.Editing.Should().BeTrue();
    }

    [Test]
    public void New_ExtensionProxy_should_Editing_false()
    {
        ExtensionProxy fixture = new(".avi");

        fixture.Editing.Should().BeFalse();
    }

    [Test]
    public void Executing_ToggleEditing_should_switch_Editing()
    {
        ExtensionProxy fixture = new(".avi");

        bool initialEditingValue = fixture.Editing;

        fixture.ToggleEditing.Execute().Subscribe();
        bool afterToggleEditing = fixture.Editing;

        fixture.ToggleEditing.Execute().Subscribe();
        bool afterSecondToggleEditing = fixture.Editing;

        initialEditingValue.Should().BeFalse();
        afterToggleEditing.Should().BeTrue();
        afterSecondToggleEditing.Should().BeFalse();
    }
}
