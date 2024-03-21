using MediatR;
using TvNoms.Server.Data.Models;

namespace TvNoms.Server.Services.Events;

public class UserSignedOut : INotification {
  public UserSignedOut(User user) {
    User = user;
  }

  public User User { get; set; }
}
