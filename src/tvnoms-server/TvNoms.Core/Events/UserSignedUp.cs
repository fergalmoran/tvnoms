using MediatR;
using TvNoms.Core.Entities;

namespace TvNoms.Core.Events;

public class UserSignedUp : INotification {
  public UserSignedUp(User user) {
    User = user;
  }

  public User User { get; set; }
}
