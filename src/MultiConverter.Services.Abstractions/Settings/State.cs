using System.Text.Json.Serialization;

namespace MultiConverter.Services.Abstractions.Settings;

public readonly record struct State(int Version = 0, [property: JsonConverter(typeof(RawJsonConverter))]
    string Value = "")
{
    public static readonly State Empty = new(0, string.Empty);

    public override string ToString()
    {
        return $"Version: {Version}, Value: {Value}";
    }
}
