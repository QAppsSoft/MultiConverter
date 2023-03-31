namespace MultiConverter.Models.Media;

/// <summary>
///     Initializes a new instance of the <see cref="Cropping" /> class.
/// </summary>
/// <param name="Top">The Top Value</param>
/// <param name="Bottom">The Bottom Value</param>
/// <param name="Left">The Left Value</param>
/// <param name="Right">The Right Value</param>
public readonly record struct Cropping(int Top, int Bottom, int Left, int Right)
{
    // /// <summary>
    // ///     Return an empty Cropping with nothing to cut
    // /// </summary>
    // /// <returns>Empty Cropping</returns>
    public static Cropping NoCrop() => new() { Top = 0, Bottom = 0, Left = 0, Right = 0 };

//
// /// <summary>
// ///     Clone this model
// /// </summary>
// /// <returns>
// ///     A Cloned copy
// /// </returns>
public Cropping Copy() => this with { };
}
