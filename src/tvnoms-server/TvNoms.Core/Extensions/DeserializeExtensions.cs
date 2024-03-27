using System.Text.Json;
using System.Text.Json.Serialization;

namespace TvNoms.Core.Extensions;

public static class DeserializeExtensions {
  private static readonly JsonSerializerOptions _config = new JsonSerializerOptions {
    NumberHandling = JsonNumberHandling.AllowReadingFromString |
                     JsonNumberHandling.WriteAsString
  };

  public static async Task<T?> DeserializeHttpContent<T>(this HttpContent content,
    CancellationToken cancellationToken) {
    var stream = await content.ReadAsStreamAsync(cancellationToken);
    var result = await stream.DeserializeAsync<T>(cancellationToken);

    return result;
  }

  public static async Task<T?> DeserializeAsync<T>(this Stream json, CancellationToken cancellationToken) {
    return await JsonSerializer.DeserializeAsync<T>(json, _config, cancellationToken);
  }
}
