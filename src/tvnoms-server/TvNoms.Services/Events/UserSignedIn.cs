using MediatR;
using TvNoms.Server.Data.Models;

namespace TvNoms.Server.Services.Events;

public class UserSignedIn : INotification {
  public UserSignedIn(User user) {
    User = user;
  }

  public User User { get; set; }
}
