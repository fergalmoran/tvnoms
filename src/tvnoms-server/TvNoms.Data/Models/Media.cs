namespace TvNoms.Server.Data.Models;

public record Media : BaseEntity {
  public string Name { get; set; } = default!;

  public long Size { get; set; }

  public string Path { get; set; } = default!;

  public string ContentType { get; set; } = default!;

  public MediaType Type { get; set; }

  public int? Width { get; set; }

  public int? Height { get; set; }

  public DateTimeOffset CreatedAt { get; set; }

  public DateTimeOffset UpdatedAt { get; set; }
}

public enum MediaType {
  Document,
  Image,
  Video,
  Audio,
  Unknown
}
