using Microsoft.AspNetCore.Identity;
using TvNoms.Server.Data;
using TvNoms.Server.Data.Models;
using TvNoms.Server.Services.Identity;

namespace TvNoms.Server.Services.Data.Repositories;

public interface IRoleRepository : IRepository<Role> {
  Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}

public class RoleRepository : AppRepository<Role>, IRoleRepository {
  private readonly RoleManager<Role> _roleManager;

  public RoleRepository(RoleManager<Role> roleManager, AppDbContext dbContext) : base(dbContext) {
    _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
  }

  public override async Task<Role> CreateAsync(Role role, CancellationToken cancellationToken = default) {
    if (role == null) throw new ArgumentNullException(nameof(role));

    var result = await _roleManager.CreateAsync(role);
    if (!result.Succeeded) throw new InvalidOperationException(result.Errors.GetMessage());
    return role;
  }

  public override async Task<Role> UpdateAsync(Role role, CancellationToken cancellationToken = default) {
    if (role == null) throw new ArgumentNullException(nameof(role));

    var result = await _roleManager.UpdateAsync(role);
    if (!result.Succeeded) throw new InvalidOperationException(result.Errors.GetMessage());
    return role;
  }

  public override async Task DeleteAsync(Role role, CancellationToken cancellationToken = default) {
    if (role == null) throw new ArgumentNullException(nameof(role));

    var result = await _roleManager.DeleteAsync(role);

    if (!result.Succeeded) throw new InvalidOperationException(result.Errors.GetMessage());
  }

  public Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default) {
    if (name == null) throw new ArgumentNullException(nameof(name));

    return _roleManager.FindByNameAsync(name);
  }
}
