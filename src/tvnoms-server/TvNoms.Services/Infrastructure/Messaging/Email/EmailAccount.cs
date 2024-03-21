namespace TvNoms.Server.Services.Infrastructure.Messaging.Email;

public class EmailAccount {
  public string Username { get; set; } = default!;

  public string Password { get; set; } = default!;

  public string Email { get; set; } = default!;

  public string DisplayName { get; set; } = default!;
}
