using TvNoms.Core.Entities;
using TvNoms.Core.Extensions.Identity;
using AbstractProfile = AutoMapper.Profile;

namespace TvNoms.Core.Models.Users;

public class UserWithSessionModel : UserModel {
  public bool EmailConfirmed { get; set; }

  public bool PhoneNumberConfirmed { get; set; }

  public string? AccessToken { get; set; }

  public string? RefreshToken { get; set; }

  public string? TokenType { get; set; }
}

public class UserWithSessionModelProfile : AbstractProfile {
  public UserWithSessionModelProfile() {
    CreateMap<User, UserWithSessionModel>();
    CreateMap<UserSessionInfo, UserWithSessionModel>();
  }
}
