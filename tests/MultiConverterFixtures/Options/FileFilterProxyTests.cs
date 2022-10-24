using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using FluentAssertions;
using MultiConverter.Models.Settings.General.FileFilters;
using MultiConverter.ViewModels.Options;
using NUnit.Framework;

namespace MultiConverterFixtures.Options;

public class FileFilterProxyTests
{
    [Test]
    public async Task New_FileFilterProxy_should_HasChanged_true()
    {
        using FileFilterProxy fixture = new();

        bool result = await fixture.HasChanged.Take(1).ToTask();

        result.Should().BeTrue();
    }

    [Test]
    public async Task FileFilterProxy_with_default_FileFilter_should_HasChanged_true()
    {
        using FileFilterProxy fixture = new(FileFilter.Default);

        bool result = await fixture.HasChanged.Take(1).ToTask();

        result.Should().BeTrue();
    }

    [Test]
    public async Task FileFilterProxy_with_non_default_FileFilter_should_HasChanged_false()
    {
        using FileFilterProxy fixture = new(FileFilter.Default with { Filter = "*.mkv" });

        bool result = await fixture.HasChanged.Take(1).ToTask();

        result.Should().BeFalse();
    }

    [Test]
    public async Task Changing_FileFilterProxy_Filter_should_HasChanged_true()
    {
        using FileFilterProxy fixture = new(FileFilter.Default with { Filter = "*.mkv" });

        fixture.Filter = "*.*";

        bool result = await fixture.HasChanged.Take(1).ToTask();

        result.Should().BeTrue();
    }

    [Test]
    public async Task Changing_FileFilterProxy_Position_should_HasChanged_true()
    {
        using FileFilterProxy fixture = new(FileFilter.Default with { Filter = "*.mkv" });

        fixture.Position = FileFilterPosition.Ends;

        bool result = await fixture.HasChanged.Take(1).ToTask();

        result.Should().BeTrue();
    }

    [Test]
    public async Task Changing_FileFilterProxy_ApplyOn_should_HasChanged_true()
    {
        using FileFilterProxy fixture = new(FileFilter.Default with { Filter = "*.mkv" });

        fixture.ApplyOn = FileFilterApplyOn.Extension;

        bool result = await fixture.HasChanged.Take(1).ToTask();

        result.Should().BeTrue();
    }

    [Test]
    public void New_FileFilterProxy_with_default_filter_should_Editing_true()
    {
        using FileFilterProxy fixture = new();

        fixture.Editing.Should().BeTrue();
    }

    [Test]
    public void New_FileFilterProxy_should_Editing_false()
    {
        using FileFilterProxy fixture = new(FileFilter.Default with { Filter = "*.mkv" });

        fixture.Editing.Should().BeFalse();
    }

    [Test]
    public void Executing_ToggleEditing_should_switch_Editing()
    {
        using FileFilterProxy fixture = new(FileFilter.Default with { Filter = "*.mkv" });

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
