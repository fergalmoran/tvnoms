using TvNoms.Server.Data;
using TvNoms.Server.Data.Models;

namespace TvNoms.Server.Services.Data.Repositories;

public interface IMediaRepository : IRepository<Media> {
}

public class MediaRepository : AppRepository<Media>, IMediaRepository {
  public MediaRepository(AppDbContext dbContext) : base(dbContext) {
  }
}
