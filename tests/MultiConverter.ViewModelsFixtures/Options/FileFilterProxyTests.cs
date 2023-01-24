using FluentAssertions;
using MultiConverter.Models.Settings.General.FileFilters;
using MultiConverter.ViewModels.Settings;

namespace MultiConverter.ViewModelsFixtures.Options;

public class FileFilterProxyTests
{
    [Test]
    public void New_FileFilterProxy_should_HasChanged_true()
    {
        using FileFilterProxy fixture = new();

        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void FileFilterProxy_with_default_FileFilter_should_HasChanged_true()
    {
        using FileFilterProxy fixture = new(FileFilter.Default);

        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void FileFilterProxy_with_non_default_FileFilter_should_HasChanged_false()
    {
        using FileFilterProxy fixture = new(FileFilter.Default with { Filter = "*.mkv" });

        fixture.HasChanged.Should().BeFalse();
    }

    [Test]
    public void Changing_FileFilterProxy_Filter_should_HasChanged_true()
    {
        using FileFilterProxy fixture = new(FileFilter.Default with { Filter = "*.mkv" });

        fixture.Filter = "*.*";

        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void Changing_FileFilterProxy_Position_should_HasChanged_true()
    {
        using FileFilterProxy fixture = new(FileFilter.Default with { Filter = "*.mkv" });

        fixture.Position = FileFilterPosition.Ends;

        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void Changing_FileFilterProxy_ApplyOn_should_HasChanged_true()
    {
        using FileFilterProxy fixture = new(FileFilter.Default with { Filter = "*.mkv" });

        fixture.ApplyOn = FileFilterApplyOn.Extension;

        fixture.HasChanged.Should().BeTrue();
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

    [Test]
    public void IEnumerable_properties_should_be_filled()
    {
        using FileFilterProxy fixture = new();

        fixture.Positions.Should().BeEquivalentTo(new[]
        {
            FileFilterPosition.Contains,
            FileFilterPosition.Ends,
            FileFilterPosition.Start
        });

        fixture.ApplyOnParts.Should().BeEquivalentTo(new[]
        {
            FileFilterApplyOn.Extension,
            FileFilterApplyOn.File,
            FileFilterApplyOn.FileAndExtension
        });
    }

    [Test]
    public void Check_implicit_conversion_to_FileFilter()
    {
        FileFilterProxy initial = new();

        FileFilter fixture = initial;

        fixture.Filter.Should().Be(initial.Filter);
        fixture.Position.Should().Be(initial.Position);
        fixture.ApplyOn.Should().Be(initial.ApplyOn);
    }

}
