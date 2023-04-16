using FluentAssertions;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Presets.Options;
using MultiConverter.ViewModels.Presets.Options;

namespace MultiConverter.ViewModelsFixtures.Presets.Options;

[TestFixture]
public class VideoAspectRatioOptionViewModelTests
{
    private const string DefaultAspectRatio = "16:9";


    [Test]
    public void New_VideoAspectRatioOptionViewModel_After_Initialization()
    {
        VideoAspectRatioOptionViewModel fixture = InitializeFixture();

        fixture.AspectRatio.Should().Be(DefaultAspectRatio);
        fixture.HasChanged.Should().BeFalse();
        fixture.DefaultOptions.Length.Should().Be(2);
    }

    [Test]
    public void After_value_changed_HasChanged_should_be_true()
    {
        const string newAspectRatio = "4:3";
        VideoAspectRatioOptionViewModel fixture = InitializeFixture();

        fixture.AspectRatio = newAspectRatio;

        fixture.HasChanged.Should().BeTrue();
        fixture.AspectRatio.Should().Be(newAspectRatio);
    }

    [Test]
    [TestCase(0, "4:3")]
    [TestCase(1, "16:9")]
    public void Executing_ValuesUpdater_should_update_value(int index, string aspectRatio)
    {
        VideoAspectRatioOptionViewModel fixture = InitializeFixture(string.Empty);
        ValuesUpdater updater = fixture.DefaultOptions[index];

        updater.Update.Invoke();

        fixture.AspectRatio.Should().Be(aspectRatio);
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void After_set_initial_value_HasChanged_should_be_false()
    {
        VideoAspectRatioOptionViewModel fixture = InitializeFixture();

        fixture.AspectRatio = "4:3";
        fixture.AspectRatio = DefaultAspectRatio;

        fixture.HasChanged.Should().BeFalse();
        fixture.AspectRatio.Should().Be(DefaultAspectRatio);
    }

    private static VideoAspectRatioOptionViewModel InitializeFixture(string? initialAspectRatio = null)
    {
        ISchedulerProvider scheduler = new ImmediateSchedulers();
        VideoAspectRatioOption option = new(initialAspectRatio ?? DefaultAspectRatio);
        return new VideoAspectRatioOptionViewModel(option, scheduler);
    }
}
