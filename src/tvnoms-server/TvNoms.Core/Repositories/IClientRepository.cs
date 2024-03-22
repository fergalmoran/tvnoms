using System.Linq.Expressions;
using TvNoms.Core.Entities;

namespace TvNoms.Core.Repositories;

public interface IClientRepository : IRepository<Client> {
  Task DeactivateAsync(Client client, CancellationToken cancellationToken = default);

  Task DeactivateManyAsync(Expression<Func<Client, bool>> predicate, CancellationToken cancellationToken = default);

  Task DeactivateAllAsync(CancellationToken cancellationToken = default);
}
