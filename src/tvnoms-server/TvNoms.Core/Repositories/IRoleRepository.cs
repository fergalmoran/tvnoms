using TvNoms.Core.Entities;

namespace TvNoms.Core.Repositories;

public interface IRoleRepository : IRepository<Role> {
  Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
