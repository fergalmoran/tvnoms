using Microsoft.Extensions.Logging;
using Quartz;
using TvNoms.Core.Jobs;
using TvNoms.Core.MediaLookup;

namespace TvNoms.Infrastructure.Jobs.Media.TheTvDb;

[DisallowConcurrentExecution]
public class GetTrendingTvShowsJob(IShowLookupService lookupService, ILogger<GetTrendingTvShowsJob> logger)
  : ITVNomsJob {
  public string JobName => "GetTrendingTvShowsJob";

  public async Task Execute(IJobExecutionContext context) {
    logger.LogDebug("Starting GetTrendingTvShowsJob");
    var results = await lookupService.GetTrendingShows();
    foreach (var result in results.Items) {
      logger.LogDebug("Found show: {ResultTitle}", result.Title);
    }
  }
}
