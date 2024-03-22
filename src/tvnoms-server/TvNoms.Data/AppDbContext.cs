using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TvNoms.Server.Data.Models;

namespace TvNoms.Server.Data;

public class AppDbContext(IConfiguration configuration) : DbContext {
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
    optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));


  public DbSet<User> Users { get; set; }
  public DbSet<Client> Clients { get; set; }
  public DbSet<Show> Shows { get; set; }
  public DbSet<Movie> Movies { get; set; }
  public DbSet<Media> Media { get; set; }
}
