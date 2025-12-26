namespace Home.AutoPlayer.Dto;

public sealed class PlayList {
    public bool? Collaborative { get; init; }

    public string? Description { get; init; }

    public Dictionary<string, string>? ExternalUrls { get; init; }
    
    public string? Href { get; init; }

    public string? Id { get; init; }
    
    public string? Name { get; init; }

    public bool? Public { get; init; }

    public string? SnapshotId { get; init; }
    
    public string? Type { get; init; }

    public string? Uri { get; init; }
}

