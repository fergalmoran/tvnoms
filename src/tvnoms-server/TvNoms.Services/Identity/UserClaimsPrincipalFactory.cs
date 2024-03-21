using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TvNoms.Server.Data.Models;

namespace TvNoms.Server.Services.Identity;

public class UserClaimsPrincipalFactory(
  UserManager<User> userManager,
  RoleManager<Role> roleManager,
  IOptions<IdentityOptions> optionsAccessor)
  : UserClaimsPrincipalFactory<User, Role>(userManager, roleManager, optionsAccessor) {
  public override async Task<ClaimsPrincipal> CreateAsync(User user) {
    var principal = await base.CreateAsync(user);

    ((ClaimsIdentity)principal.Identity!).AddClaims(new Claim[] {
      new(ClaimTypes.GivenName, user.FirstName),
      new(ClaimTypes.Surname, user.LastName)
    });

    return principal;
  }
}
