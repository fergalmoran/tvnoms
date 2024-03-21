using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TvNoms.Server.Data.Models;

namespace TvNoms.Server.Data;

public class AppDbContext : DbContext {
  private readonly IConfiguration _configuration;

  public AppDbContext(IConfiguration configuration) {
    _configuration = configuration;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));

  public DbSet<Show> Shows { get; set; }
  public DbSet<Movie> Movies { get; set; }
}
