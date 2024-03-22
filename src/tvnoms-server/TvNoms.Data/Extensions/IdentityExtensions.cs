using Microsoft.AspNetCore.Identity;

namespace TvNoms.Server.Data.Extensions;

public static class IdentityExtensions {
  public static string GetMessage(this IEnumerable<IdentityError> errors) {
    return "Operation failed: " + string.Join(string.Empty,
      errors.Select(x => $"{Environment.NewLine} -- {x.Code}: {x.Description}"));
  }
}
