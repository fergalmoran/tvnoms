using MediatR;
using TvNoms.Core.Entities;

namespace TvNoms.Core.Events;

public class UserSignedOut : INotification {
  public UserSignedOut(User user) {
    User = user;
  }

  public User User { get; set; }
}
