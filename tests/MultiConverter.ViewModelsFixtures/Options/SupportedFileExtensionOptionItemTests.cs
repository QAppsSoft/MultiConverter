using FluentAssertions;
using MultiConverter.Models.Settings.General;
using MultiConverter.ViewModels.Settings;

namespace MultiConverter.ViewModelsFixtures.Options;

public class SupportedFileExtensionOptionItemTests : OptionsTestBase
{
    [Test]
    public void After_initialize()
    {
        bool? canDelete = null;
        bool? canAdd = null;
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using var fixture = mocker.CreateInstance<SupportedFileExtensionSettingItem>();

        fixture.Delete.CanExecute.Subscribe(x => canDelete = x);
        fixture.Add.CanExecute.Subscribe(x => canAdd = x);

        fixture.HasChanged.Should().BeFalse();
        fixture.SupportedExtensions.Select(x => (string)x).Should()
            .BeEquivalentTo(GeneralOptions.Default().SupportedFilesExtensions);
        canDelete.Should().BeTrue();
        canAdd.Should().BeTrue();
    }

    [Test]
    public void After_adding_new_item_hasChanged_and_cannot_add()
    {
        bool? canDelete = null;
        bool? canAdd = null;
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using var fixture = mocker.CreateInstance<SupportedFileExtensionSettingItem>();

        fixture.Add.Execute().Subscribe();
        fixture.Delete.CanExecute.Subscribe(x => canDelete = x);
        fixture.Add.CanExecute.Subscribe(x => canAdd = x);

        fixture.SupportedExtensions.Select(x => (string)x).Should()
            .NotBeEquivalentTo(GeneralOptions.Default().SupportedFilesExtensions);
        canAdd.Should().BeFalse();
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void After_adding_new_item_and_extension_modified_hasChanged_and_can_add()
    {
        bool? canDelete = null;
        bool? canAdd = null;
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using var fixture = mocker.CreateInstance<SupportedFileExtensionSettingItem>();

        fixture.Add.Execute().Subscribe();
        var extensionProxy = fixture.SupportedExtensions.First(x => x.Extension == string.Empty);
        extensionProxy.Extension = ".exe";
        fixture.Delete.CanExecute.Subscribe(x => canDelete = x);
        fixture.Add.CanExecute.Subscribe(x => canAdd = x);

        fixture.SupportedExtensions.Select(x => (string)x).Should()
            .NotBeEquivalentTo(GeneralOptions.Default().SupportedFilesExtensions);
        canAdd.Should().BeTrue();
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void With_clear_list_cannot_delete()
    {
        bool? canDelete = null;
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker, GeneralOptions.Default() with { SupportedFilesExtensions = Array.Empty<string>() });
        using var fixture = mocker.CreateInstance<SupportedFileExtensionSettingItem>();

        fixture.Delete.CanExecute.Subscribe(x => canDelete = x);

        canDelete.Should().BeFalse();
    }

    [Test]
    public void Reset_should_apply_default_options()
    {
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker, GeneralOptions.Default() with { SupportedFilesExtensions = Array.Empty<string>() });
        using var fixture = mocker.CreateInstance<SupportedFileExtensionSettingItem>();

        fixture.Reset.Execute().Subscribe();

        fixture.SupportedExtensions.Select<ExtensionProxy, string>(x => x).Should()
            .BeEquivalentTo(GeneralOptions.Default().SupportedFilesExtensions);
    }

    [Test]
    public void Delete_one_item()
    {
        int expected = GeneralOptions.Default().SupportedFilesExtensions.Length - 1;
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using var fixture = mocker.CreateInstance<SupportedFileExtensionSettingItem>();

        var toDelete = fixture.SupportedExtensions.Last();
        fixture.Delete.Execute(toDelete).Subscribe();

        fixture.SupportedExtensions.Count.Should().Be(expected);
    }

    [Test]
    public void UpdateOption_should_update_provided_GeneralOptions()
    {
        int expected = GeneralOptions.Default().SupportedFilesExtensions.Length - 1;
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using var fixture = mocker.CreateInstance<SupportedFileExtensionSettingItem>();

        var toDelete = fixture.SupportedExtensions.Last();
        fixture.Delete.Execute(toDelete).Subscribe();
        var result = fixture.UpdateOption.Invoke(GeneralOptions.Default());

        result.SupportedFilesExtensions.Length.Should().Be(expected);
    }

    [Test]
    public void After_deleting_item_hasChanged_should_be_true()
    {
        var mocker = GetAutoMocker();
        SetupGeneralOptions(mocker);
        using var fixture = mocker.CreateInstance<SupportedFileExtensionSettingItem>();

        ExtensionProxy toDelete = fixture.SupportedExtensions.First();
        fixture.Delete.Execute(toDelete).Subscribe();

        fixture.HasChanged.Should().BeTrue();
    }
}
