namespace TvNoms.Core.Extensions.Identity;

public class UserSessionOptions {
  public string Secret { set; get; } = default!;

  public string Issuer { set; get; } = default!;

  public string Audience { set; get; } = default!;

  public TimeSpan AccessTokenExpiresIn { set; get; }

  public TimeSpan RefreshTokenExpiresIn { set; get; }

  public bool AllowMultipleSessions { set; get; }


  public const string ValueSeparator = ";";

  public IEnumerable<string> GetIssuers() {
    return Issuer?.Split(ValueSeparator, StringSplitOptions.RemoveEmptyEntries).ToArray() ?? Array.Empty<string>();
  }

  public IEnumerable<string> GetAudiences() {
    return Audience?.Split(ValueSeparator, StringSplitOptions.RemoveEmptyEntries).ToArray() ?? Array.Empty<string>();
  }
}
