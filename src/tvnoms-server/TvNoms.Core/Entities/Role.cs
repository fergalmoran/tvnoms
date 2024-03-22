using Microsoft.AspNetCore.Identity;

namespace TvNoms.Core.Entities;

public class Role : IdentityRole<Guid>, IEntity {
  public Role() {
  }

  public Role(string roleName) : base(roleName) {
  }


  public const string Admin = nameof(Admin);

  public const string Member = nameof(Member);

  public static IEnumerable<string> All => new[] { Admin, Member };
}
