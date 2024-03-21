using Microsoft.AspNetCore.Identity;

namespace TvNoms.Server.Data.Models;

public class User : IdentityUser<Guid>, IEntity {
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public Media? Avatar { get; set; }
  public Guid? AvatarId { get; set; }
  public string? Bio { get; set; }
  public string? Location { get; set; }
  public bool Active { get; set; }
  public DateTimeOffset LastActiveAt { get; set; }
  public virtual ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
  public virtual ICollection<Client> Clients { get; set; } = new List<Client>();


  public bool EmailRequired { get; set; }
  public bool PhoneNumberRequired { get; set; }
}

public class UserRole : IdentityUserRole<Guid>, IEntity {
  public virtual User User { get; set; } = default!;

  public virtual Role Role { get; set; } = default!;

  Guid IEntity.Id { get; set; }
}

public class UserSession : IEntity {
  public virtual User User { get; set; } = default!;

  public Guid UserId { get; set; }

  public Guid Id { get; set; }

  public string AccessTokenHash { get; set; } = default!;

  public DateTimeOffset AccessTokenExpiresAt { get; set; }

  public string RefreshTokenHash { get; set; } = default!;

  public DateTimeOffset RefreshTokenExpiresAt { get; set; }
}

public class Role : IdentityRole<Guid>, IEntity {
  public Role() {
  }

  public Role(string roleName) : base(roleName) {
  }

  public virtual ICollection<UserRole> Users { get; set; } = new List<UserRole>();

  public const string Admin = nameof(Admin);

  public const string Member = nameof(Member);

  public static IEnumerable<string> All => new[] { Admin, Member };
}
