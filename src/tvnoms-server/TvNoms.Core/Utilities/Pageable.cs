using System.Collections;
using System.Text.Json.Serialization;

namespace TvNoms.Core.Utilities;

public class Pageable<T> : IPageable<T> {
  public Pageable() {
  }

  public Pageable(long offset, int limit, long total, List<T> items) {
    Offset = offset;
    Limit = limit;
    Length = total;
    Items = items;
  }

  [JsonPropertyName("Offset")]
  public long Offset { get; }
  public int Limit { get; }
  public long Length { get; }
  public IList<T>? Items { get; } = new List<T>();
  public long? Previous => Offset - Limit >= 0 ? Offset - Limit : null;
  public long? Next => Offset + Limit < Length ? Offset + Limit : null;

  public IEnumerator<T> GetEnumerator() {
    return Items?.GetEnumerator() ?? new List<T>.Enumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }
}
