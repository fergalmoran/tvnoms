using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TvNoms.Core.Entities;
using TvNoms.Core.Utilities;
using TvNoms.Server.Data.Extensions;

namespace TvNoms.Server.Data;

public class AppDbContext(IConfiguration configuration) :
  IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>,
    UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>> {
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
    optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

  protected override void OnModelCreating(ModelBuilder builder) {
    base.OnModelCreating(builder);
    var assemblies = AssemblyHelper.GetAssemblies();

    builder.ApplyEntities(assemblies);
    builder.ApplyConfigurations(assemblies);
  }

  // public DbSet<User> Users { get; set; }
// public DbSet<UserRole> UserRoles { get; set; }
// public DbSet<Client> Clients { get; set; }
// public DbSet<Show> Shows { get; set; }
// public DbSet<Movie> Movies { get; set; }
// public DbSet<Media> Media { get; set; }
}
