using MediatR;
using TvNoms.Core.Entities;

namespace TvNoms.Core.Events;

public class UserSignedIn : INotification {
  public UserSignedIn(User user) {
    User = user;
  }

  public User User { get; set; }
}
