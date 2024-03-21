using System.Collections;

namespace TvNoms.Server.Services.Data.Utilities;

public class Pageable<T> : IPageable<T> {
  public Pageable(long offset, int limit, long total, IEnumerable<T> items) {
    Offset = offset;
    Limit = limit;
    Length = total;
    Items = items;
  }

  public long Offset { get; }
  public int Limit { get; }
  public long Length { get; }
  public IEnumerable<T> Items { get; }
  public long? Previous => Offset - Limit >= 0 ? Offset - Limit : null;
  public long? Next => Offset + Limit < Length ? Offset + Limit : null;

  public IEnumerator<T> GetEnumerator() {
    return Items.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }
}
