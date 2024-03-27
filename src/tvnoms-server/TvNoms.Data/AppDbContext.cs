using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TvNoms.Core.Entities;

namespace TvNoms.Server.Data;

public class AppDbContext(IConfiguration configuration) :
  IdentityDbContext<User, IdentityRole<Guid>, Guid> {
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
    optionsBuilder
      .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
      .UseSnakeCaseNamingConvention();

  protected override void OnModelCreating(ModelBuilder builder) {
    base.OnModelCreating(builder);
    builder.HasDefaultSchema("tvnoms");
    builder.UseIdentityByDefaultColumns();
    //give the identity tables proper names and schema
    builder.Entity<User>().ToTable("user", "auth");
    builder.Entity<IdentityUser>().ToTable("identity_user_base", "auth");
    builder.Entity<IdentityUser<Guid>>().ToTable("identity_user", "auth");
    builder.Entity<IdentityRole<Guid>>().ToTable("user_user_role", "auth");
    builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claim", "auth");
    builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_login", "auth");
    builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claim", "auth");
    builder.Entity<IdentityUserToken<Guid>>().ToTable("user_token", "auth");
    builder.Entity<IdentityUserRole<Guid>>().ToTable("user_identity_role", "auth");
    //end identity stuff
    builder.Entity<Movie>(m => {
      m.HasMany(x => x.Genres)
        .WithMany()
        .UsingEntity(m => m.ToTable("__movie_genres"));
    });
    builder.Entity<Show>(m => {
      m.HasMany(x => x.Genres)
        .WithMany()
        .UsingEntity(m => m.ToTable("__show_genres"));
    });
  }

  public DbSet<User> Users { get; set; }
  public DbSet<UserSession> UserSessions { get; set; }
  public DbSet<Role> Roles { get; set; }
  public DbSet<Client> Clients { get; set; }
  public DbSet<Show> Shows { get; set; }
  public DbSet<Movie> Movies { get; set; }
  public DbSet<Media> Media { get; set; }
  public DbSet<Genre> Genres { get; set; }
}
