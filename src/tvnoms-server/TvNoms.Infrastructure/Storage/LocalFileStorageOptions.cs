namespace TvNoms.Infrastructure.Storage;

public class LocalFileStorageOptions {
  public string RootPath { get; set; } = default!;

  public string WebRootPath { get; set; } = default!;
}
