using FluentAssertions;
using MultiConverter.Common;
using MultiConverter.Common.Testing;
using MultiConverter.Models.Presets.Options;
using MultiConverter.ViewModels.Presets.Options;

namespace MultiConverter.ViewModelsFixtures.Presets.Options;

[TestFixture]
public class VideoSizeOptionViewModelTests
{
    private static readonly VideoSize s_defaultSize = new(3000, 3000);

    [Test]
    public void New_VideoFrameRateOptionViewModel_After_Initialization()
    {
        VideoSizeOptionViewModel fixture = InitializeFixture();

        fixture.Height.Should().Be(s_defaultSize.Height);
        fixture.Width.Should().Be(s_defaultSize.Width);
        fixture.HasChanged.Should().BeFalse();
        fixture.DefaultOptions.Length.Should().Be(9);
    }

    [Test]
    public void After_value_changed_HasChanged_should_be_true()
    {
        const int newWidth = 640;
        const int newHeight = 640;
        VideoSizeOptionViewModel fixture = InitializeFixture();

        fixture.Height = newHeight;
        fixture.Width = newWidth;

        fixture.HasChanged.Should().BeTrue();
        fixture.Width.Should().Be(newWidth);
        fixture.Height.Should().Be(newHeight);
    }

    [Test]
    [TestCase(0, 320, 240)]
    [TestCase(1, 352, 240)]
    [TestCase(2, 400, 300)]
    [TestCase(3, 640, 480)]
    [TestCase(4, 720, 480)]
    [TestCase(5, 800, 600)]
    [TestCase(6, 1024, 768)]
    [TestCase(7, 1600, 1200)]
    [TestCase(8, 1920, 1080)]
    public void Executing_ValuesUpdater_should_update_value(int index, int width, int height)
    {
        VideoSizeOptionViewModel fixture = InitializeFixture(s_defaultSize);
        ValuesUpdater updater = fixture.DefaultOptions[index];

        updater.Update.Invoke();

        fixture.Width.Should().Be(width);
        fixture.Height.Should().Be(height);
        fixture.HasChanged.Should().BeTrue();
    }

    [Test]
    public void After_set_initial_value_HasChanged_should_be_false()
    {
        VideoSizeOptionViewModel fixture = InitializeFixture();

        fixture.Width = 120;
        fixture.Height = 120;
        fixture.Width = s_defaultSize.Width;
        fixture.Height = s_defaultSize.Height;

        fixture.HasChanged.Should().BeFalse();
        fixture.Width.Should().Be(s_defaultSize.Width);
        fixture.Height.Should().Be(s_defaultSize.Height);
    }

    private static VideoSizeOptionViewModel InitializeFixture(VideoSize? initialVideoSize = null)
    {
        ISchedulerProvider scheduler = new ImmediateSchedulers();
        VideoSizeOption option = new(initialVideoSize ?? s_defaultSize);
        return new VideoSizeOptionViewModel(option, scheduler);
    }
}
