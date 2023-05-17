using MultiConverter.Models.Presets.Subtitles;
using MultiConverter.ViewModels.Presets.Subtitles;

namespace MultiConverter.ViewModels.Presets.Interfaces;

public interface ISubtitleStyleViewModelFactory
{
    SubtitleStyleViewModel Build(SubtitleStyle subtitleStyle);
}
