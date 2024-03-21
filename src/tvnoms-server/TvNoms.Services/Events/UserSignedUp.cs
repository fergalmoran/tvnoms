using MediatR;
using TvNoms.Server.Data.Models;

namespace TvNoms.Server.Services.Events;

public class UserSignedUp : INotification {
  public UserSignedUp(User user) {
    User = user;
  }

  public User User { get; set; }
}
