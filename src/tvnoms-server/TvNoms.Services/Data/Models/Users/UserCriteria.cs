using System.Linq.Expressions;
using TvNoms.Server.Data.Models;
using TvNoms.Server.Services.Utilities;

namespace TvNoms.Server.Services.Data.Models.Users;

public class UserCriteria {
  public Guid[]? Id { get; set; }

  public bool? Online { get; set; }

  public Expression<Func<User, bool>> Build() {
    var predicate = PredicateBuilder.True<User>();

    if (Id != null && Id.Any()) {
      predicate = predicate.And(user => Id.Contains(user.Id));
    }

    if (!Online.HasValue) {
      return predicate;
    }


    predicate = Online.Value
      ? predicate.And(user => user.Clients.Any(_ => _.Active))
      : predicate.And(user => !user.Clients.Any(_ => _.Active));


    return predicate;
  }
}
