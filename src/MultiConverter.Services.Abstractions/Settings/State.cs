namespace MultiConverter.Services.Abstractions.Settings;

public readonly record struct State(int Version = 0, string Value = "")
{
    public static readonly State Empty = new State(0, string.Empty);
    
    public override string ToString()
    {
        return $"Version: {Version}, Value: {Value}";
    }
}