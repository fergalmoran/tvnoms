namespace TvNoms.Server.Services.Data.Utilities;

public static class UriExtensions {
  public static Uri CombinePaths(this Uri uri, params string[] paths) {
    return new Uri(paths.SelectMany(path => path.Split('/', StringSplitOptions.RemoveEmptyEntries)).Aggregate(
      uri.AbsoluteUri, (current, path) => $"{current.TrimEnd('/')}/{path.TrimStart('/')}"));
  }
}
