namespace Home.Theming.Object;

public sealed class ThemeBackground {
    public required Stream Content { get; init; }
    public required string ContentType { get; init; }
    public long? Length { get; init; } // null if unknown
    public required string ETag { get; init; }
    public DateTimeOffset? LastModified { get; init; }
    public bool SupportsRanges { get; init; } // true only if stream is seekable
}
