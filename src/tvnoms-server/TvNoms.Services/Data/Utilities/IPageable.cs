namespace TvNoms.Server.Services.Data.Utilities;

public interface IPageable<T> : IEnumerable<T> {
  long Offset { get; }
  int Limit { get; }
  long Length { get; }
  long? Previous { get; }
  long? Next { get; }
}
