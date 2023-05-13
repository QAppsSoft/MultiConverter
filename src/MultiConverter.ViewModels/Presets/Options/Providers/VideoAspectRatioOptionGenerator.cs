using MultiConverter.Models.Presets.Interfaces;
using MultiConverter.Models.Presets.Options;

namespace MultiConverter.ViewModels.Presets.Options.Providers;

public sealed class VideoAspectRatioOptionGenerator : OptionGeneratorBase
{
    public override string Caption { get; init; } = "Video aspect ratio";
    public override IOption Generate() => new VideoAspectRatioOption("16:9");
}
