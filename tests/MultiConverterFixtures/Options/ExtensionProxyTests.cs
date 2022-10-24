using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using FluentAssertions;
using MultiConverter.ViewModels.Options;
using NUnit.Framework;

namespace MultiConverterFixtures.Options;

public class ExtensionProxyTests
{
    [Test]
    public async Task New_ExtensionProxy_should_HasChanged_true()
    {
        ExtensionProxy fixture = new();

        bool result = await fixture.HasChanged.Take(1).ToTask();

        result.Should().BeTrue();
    }

    [Test]
    public async Task ExtensionProxy_with_empty_extension_should_HasChanged_true()
    {
        ExtensionProxy fixture = new(string.Empty);

        bool result = await fixture.HasChanged.Take(1).ToTask();

        result.Should().BeTrue();
    }

    [Test]
    public async Task ExtensionProxy_with_non_empty_extension_should_HasChanged_false()
    {
        ExtensionProxy fixture = new(".avi");

        bool result = await fixture.HasChanged.Take(1).ToTask();

        result.Should().BeFalse();
    }

    [Test]
    public async Task Changing_ExtensionProxy_Extension_should_HasChanged_true()
    {
        ExtensionProxy fixture = new(".avi");

        fixture.Extension = ".mkv";

        bool result = await fixture.HasChanged.Take(1).ToTask();

        result.Should().BeTrue();
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
