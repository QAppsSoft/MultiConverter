using FluentAssertions;
using MultiConverter.Models.Settings.General;
using MultiConverter.Models.Settings.General.FileFilters;
using MultiConverter.ViewModels.Settings;

namespace MultiConverter.ViewModelsFixtures.Settings;

public class FileFiltersOptionItemTests : OptionsTestBase
{
    [Test]
    public void After_initialize()
    {
        bool? canDelete = null;
        bool? canAdd = null;
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using var fixture = mocker.CreateInstance<FileFiltersSettingItem>();

        fixture.Delete.CanExecute.Subscribe(x => canDelete = x);
        fixture.Add.CanExecute.Subscribe(x => canAdd = x);

        fixture.HasChanged.Should().BeFalse();
        fixture.FileFilters.Select(x => (FileFilter)x).Should()
            .BeEquivalentTo(GeneralOptions.Default().FileFilters);
        canDelete.Should().BeFalse();
        canAdd.Should().BeTrue();
    }

    [Test]
    public void After_initialize_with_nonEmpty_fileFilters()
    {
        var expected = new[] { new FileFilter("avi", FileFilterPosition.Contains, FileFilterApplyOn.Extension) };
        bool? canDelete = null;
        bool? canAdd = null;
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker, GeneralOptions.Default() with { FileFilters = expected });
        using var fixture = mocker.CreateInstance<FileFiltersSettingItem>();

        fixture.Delete.CanExecute.Subscribe(x => canDelete = x);
        fixture.Add.CanExecute.Subscribe(x => canAdd = x);

        fixture.HasChanged.Should().BeFalse();
        fixture.FileFilters.Select(x => (FileFilter)x).Should().BeEquivalentTo(expected);
        canDelete.Should().BeTrue();
        canAdd.Should().BeTrue();
    }

    [Test]
    public void After_changing_FileFilter_has_changed_should_be_true()
    {
        var fileFilters = new[] { new FileFilter("avi", FileFilterPosition.Contains, FileFilterApplyOn.Extension) };
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker, GeneralOptions.Default() with { FileFilters = fileFilters });
        using var fixture = mocker.CreateInstance<FileFiltersSettingItem>();

        var fileFilterItem = fixture.FileFilters.First();
        fileFilterItem.Position = FileFilterPosition.Ends;

        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void If_FileFilters_isEmpty_Delete_CanExecute_should_be_false()
    {
        bool? canDelete = null;
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using var fixture = mocker.CreateInstance<FileFiltersSettingItem>();

        fixture.Delete.CanExecute.Subscribe(x => canDelete = x);

        fixture.HasChanged.Should().BeFalse();
        fixture.FileFilters.Should().BeEmpty();
        canDelete.Should().BeFalse();
    }

    [Test]
    public void If_FileFilters_isNotEmpty_Delete_CanExecute_should_be_true()
    {
        FileFilter[] fileFilters = { new("avi", FileFilterPosition.Contains, FileFilterApplyOn.Extension) };
        bool? canDelete = null;
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker, GeneralOptions.Default() with { FileFilters = fileFilters });
        using var fixture = mocker.CreateInstance<FileFiltersSettingItem>();

        fixture.Delete.CanExecute.Subscribe(x => canDelete = x);

        fixture.HasChanged.Should().BeFalse();
        fixture.FileFilters.Should().NotBeEmpty();
        canDelete.Should().BeTrue();
    }

    [Test]
    public void Executing_Delete_should_drop_an_item()
    {
        FileFilter[] fileFilters = {
            new("avi", FileFilterPosition.Ends, FileFilterApplyOn.Extension),
            new("pic", FileFilterPosition.Contains, FileFilterApplyOn.File),
            new("done.avi", FileFilterPosition.Ends, FileFilterApplyOn.FileAndExtension),
        };
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker, GeneralOptions.Default() with { FileFilters = fileFilters });
        using var fixture = mocker.CreateInstance<FileFiltersSettingItem>();

        FileFilterProxy toDeleteFilterProxy = fixture.FileFilters[1];
        fixture.Delete.Execute(toDeleteFilterProxy).Subscribe();

        fixture.HasChanged.Should().BeFalse();
        fixture.FileFilters.Should().NotBeEmpty();
        fixture.FileFilters.Count.Should().Be(2);
        fixture.FileFilters.Should().NotContain(toDeleteFilterProxy);
    }

    [Test]
    public void Executing_reset_should_reset_to_default()
    {
        FileFilter[] fileFilters = {
            new("avi", FileFilterPosition.Ends, FileFilterApplyOn.Extension),
            new("pic", FileFilterPosition.Contains, FileFilterApplyOn.File),
            new("done.avi", FileFilterPosition.Ends, FileFilterApplyOn.FileAndExtension),
        };
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker, GeneralOptions.Default() with { FileFilters = fileFilters });
        using var fixture = mocker.CreateInstance<FileFiltersSettingItem>();

        fixture.Reset.Execute().Subscribe();

        fixture.HasChanged.Should().BeFalse();
        fixture.FileFilters.Should().BeEquivalentTo(GeneralOptions.Default().FileFilters);
    }

    [Test]
    public void Executing_add_should_add_new_item()
    {
        FileFilter expected = FileFilter.Default;
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker, GeneralOptions.Default() with { FileFilters = Array.Empty<FileFilter>() });
        using var fixture = mocker.CreateInstance<FileFiltersSettingItem>();

        fixture.Add.Execute().Subscribe();

        fixture.HasChanged.Should().BeTrue();
        fixture.FileFilters.Count.Should().Be(1);
        fixture.FileFilters.Select<FileFilterProxy, FileFilter>(x => x).Should().Contain(expected);
    }

    [Test]
    public void Add_cannot_execute_if_FileFilters_contains_default_FileFilter()
    {
        FileFilter defaultFileFilter = FileFilter.Default;
        bool? canAdd = null;
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker, GeneralOptions.Default() with { FileFilters = new[] { defaultFileFilter } });
        using var fixture = mocker.CreateInstance<FileFiltersSettingItem>();

        fixture.Add.CanExecute.Subscribe(x => canAdd = x);

        canAdd.Should().BeFalse();
    }

    [Test]
    public void UpdateOption_should_update_GeneralOptions()
    {
        FileFilter fileFilter = FileFilter.Default with { Filter = "avi" };
        GeneralOptions expected = GeneralOptions.Default() with { FileFilters = new[] { fileFilter } };
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using var fixture = mocker.CreateInstance<FileFiltersSettingItem>();

        fixture.Add.Execute().Subscribe();
        fixture.FileFilters.First().Filter = "avi";

        fixture.UpdateOption(GeneralOptions.Default()).Should().Be(expected);
    }

    [Test]
    public void Changing_FileFilters_should_set_hasChanges_true()
    {
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using var fixture = mocker.CreateInstance<FileFiltersSettingItem>();

        fixture.Add.Execute().Subscribe();
        fixture.FileFilters.First().Filter = "avi";

        fixture.HasChanged.Should().BeTrue();
    }
}
