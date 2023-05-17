using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using MultiConverter.Common;
using MultiConverter.Extensions;
using MultiConverter.Models.Presets.Enums;
using MultiConverter.Models.Presets.Subtitles;
using MultiConverter.ViewModels.Presets.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MultiConverter.ViewModels.Presets.Subtitles;

public sealed class SubtitleStyleViewModel : ViewModelBase, IChanged, IDisposable
{
    private readonly CompositeDisposable _cleanup = new();

    public SubtitleStyleViewModel(SubtitleStyle subtitleStyle, ISchedulerProvider schedulerProvider)
    {
        ArgumentNullException.ThrowIfNull(subtitleStyle);
        ArgumentNullException.ThrowIfNull(schedulerProvider);

        InitializeProperties(subtitleStyle);

        HasChangedObservable(subtitleStyle).ToPropertyEx(this, vm => vm.HasChanged).DisposeWith(_cleanup);
    }

    [Reactive] public string FontName { get; set; }

    [Reactive] public int FontSize { get; set; }

    [Reactive] public AssColor PrimaryColour { get; set; }

    [Reactive] public AssColor SecondaryColour { get; set; }

    [Reactive] public AssColor OutlineColour { get; set; }

    [Reactive] public AssColor BackColour { get; set; }

    [Reactive] public bool Bold { get; set; }

    [Reactive] public bool Italic { get; set; }

    [Reactive] public bool Underline { get; set; }

    [Reactive] public bool StrikeOut { get; set; }

    [Reactive] public int ScaleX { get; set; }

    [Reactive] public int ScaleY { get; set; }

    [Reactive] public int Spacing { get; set; }

    [Reactive] public int Angle { get; set; }

    [Reactive] public SubtitleBorderStyle SelectedBorderStyle { get; set; }

    [Reactive] public int Outline { get; set; }

    [Reactive] public int Shadow { get; set; }

    [Reactive] public SubtitleAlignment SelectedAlignment { get; set; }

    [Reactive] public int MarginL { get; set; }

    [Reactive] public int MarginR { get; set; }

    [Reactive] public int MarginV { get; set; }

    public string[] Fonts { get; } = { "Arial", "Tahoma", "Calibri" };

    public SubtitleBorderStyle[] BorderStyles { get; } =
    {
        SubtitleBorderStyle.Outline, SubtitleBorderStyle.OpaqueBox
    };

    public SubtitleAlignment[] Alignments { get; } =
    {
        SubtitleAlignment.Left, SubtitleAlignment.Centered, SubtitleAlignment.Right, SubtitleAlignment.LeftTop,
        SubtitleAlignment.CenteredTop, SubtitleAlignment.RightTop, SubtitleAlignment.LeftMidTitle,
        SubtitleAlignment.CenteredMidTitle, SubtitleAlignment.RightMidTitle
    };

    [ObservableAsProperty] public bool HasChanged { get; }

    private IObservable<bool> HasChangedObservable(SubtitleStyle subtitleStyle) =>
        new[]
        {
            this.WhenAnyValue(vm => vm.FontName).Select(x => x != subtitleStyle.FontName),
            this.WhenAnyValue(vm => vm.FontSize).Select(x => x != subtitleStyle.FontSize),
            this.WhenAnyValue(vm => vm.PrimaryColour).Select(x => x != subtitleStyle.PrimaryColour),
            this.WhenAnyValue(vm => vm.SecondaryColour).Select(x => x != subtitleStyle.SecondaryColour),
            this.WhenAnyValue(vm => vm.OutlineColour).Select(x => x != subtitleStyle.OutlineColour),
            this.WhenAnyValue(vm => vm.BackColour).Select(x => x != subtitleStyle.BackColour),
            this.WhenAnyValue(vm => vm.Bold).Select(x => x != subtitleStyle.Bold),
            this.WhenAnyValue(vm => vm.Italic).Select(x => x != subtitleStyle.Italic),
            this.WhenAnyValue(vm => vm.Underline).Select(x => x != subtitleStyle.Underline),
            this.WhenAnyValue(vm => vm.StrikeOut).Select(x => x != subtitleStyle.StrikeOut),
            this.WhenAnyValue(vm => vm.ScaleX).Select(x => x != subtitleStyle.ScaleX),
            this.WhenAnyValue(vm => vm.ScaleY).Select(x => x != subtitleStyle.ScaleY),
            this.WhenAnyValue(vm => vm.Spacing).Select(x => x != subtitleStyle.Spacing),
            this.WhenAnyValue(vm => vm.Angle).Select(x => x != subtitleStyle.Angle),
            this.WhenAnyValue(vm => vm.SelectedBorderStyle).Select(x => x != subtitleStyle.BorderStyle),
            this.WhenAnyValue(vm => vm.Outline).Select(x => x != subtitleStyle.Outline),
            this.WhenAnyValue(vm => vm.Shadow).Select(x => x != subtitleStyle.Shadow),
            this.WhenAnyValue(vm => vm.SelectedAlignment).Select(x => x != subtitleStyle.Alignment),
            this.WhenAnyValue(vm => vm.MarginL).Select(x => x != subtitleStyle.MarginL),
            this.WhenAnyValue(vm => vm.MarginR).Select(x => x != subtitleStyle.MarginR),
            this.WhenAnyValue(vm => vm.MarginV).Select(x => x != subtitleStyle.MarginV),
        }.CombineLatest(statuses => statuses.AnyIsTrue());

    private void InitializeProperties(SubtitleStyle subtitleStyle)
    {
        FontName = subtitleStyle.FontName;
        FontSize = subtitleStyle.FontSize;
        PrimaryColour = subtitleStyle.PrimaryColour;
        SecondaryColour = subtitleStyle.SecondaryColour;
        OutlineColour = subtitleStyle.OutlineColour;
        BackColour = subtitleStyle.BackColour;
        Bold = subtitleStyle.Bold;
        Italic = subtitleStyle.Italic;
        Underline = subtitleStyle.Underline;
        StrikeOut = subtitleStyle.StrikeOut;
        ScaleX = subtitleStyle.ScaleX;
        ScaleY = subtitleStyle.ScaleY;
        Spacing = subtitleStyle.Spacing;
        Angle = subtitleStyle.Angle;
        SelectedBorderStyle = subtitleStyle.BorderStyle;
        Outline = subtitleStyle.Outline;
        Shadow = subtitleStyle.Shadow;
        SelectedAlignment = subtitleStyle.Alignment;
        MarginL = subtitleStyle.MarginL;
        MarginR = subtitleStyle.MarginR;
        MarginV = subtitleStyle.MarginV;
    }

    public static implicit operator SubtitleStyle(SubtitleStyleViewModel vm) => new(
        vm.FontName,
        vm.FontSize,
        vm.PrimaryColour,
        vm.SecondaryColour,
        vm.OutlineColour,
        vm.BackColour,
        vm.Bold,
        vm.Italic,
        vm.Underline,
        vm.StrikeOut,
        vm.ScaleX,
        vm.ScaleY,
        vm.Spacing,
        vm.Angle,
        vm.SelectedBorderStyle,
        vm.Outline,
        vm.Shadow,
        vm.SelectedAlignment,
        vm.MarginL,
        vm.MarginR,
        vm.MarginV
    );

    public void Dispose() => _cleanup.Dispose();
}
