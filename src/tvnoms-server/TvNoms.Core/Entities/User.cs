using Microsoft.AspNetCore.Identity;

namespace TvNoms.Core.Entities;

public class User : IdentityUser<Guid>, IEntity {
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public Media? Avatar { get; set; }
  public Guid? AvatarId { get; set; }
  public string? Bio { get; set; }
  public string? Location { get; set; }
  public bool Active { get; set; }
  public DateTimeOffset LastActiveAt { get; set; }
  public virtual ICollection<Client> Clients { get; set; } = new List<Client>();


  public bool EmailRequired { get; set; }
  public bool PhoneNumberRequired { get; set; }
}


public class UserSession : IEntity {
  public Guid Id { get; set; }

  public virtual User User { get; set; } = default!;
  public Guid UserId { get; set; }


  public string AccessTokenHash { get; set; } = default!;
  public DateTimeOffset AccessTokenExpiresAt { get; set; }
  public string RefreshTokenHash { get; set; } = default!;
  public DateTimeOffset RefreshTokenExpiresAt { get; set; }
}
