namespace Home.Config;

public class Plug {
    public required string Identifier { get; init; }
    public required SourceType SourceType { get; init; }
    public required string Name { get; init; }
    public required bool PowerControl { get; init; }
    public required bool PowerUsage { get; init; }
}
