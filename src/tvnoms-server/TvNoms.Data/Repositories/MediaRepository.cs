using TvNoms.Core.Entities;
using TvNoms.Core.Repositories;

namespace TvNoms.Server.Data.Repositories;



public class MediaRepository : AppRepository<Media>, IMediaRepository {
  public MediaRepository(AppDbContext dbContext) : base(dbContext) {
  }
}
