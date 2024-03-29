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
    return;
    //for now forget caching them, let's just use the API as we're only
    //making a few requests per day
    var results = await lookupService.GetTrendingShows();
    foreach (var result in results.Items) {
      logger.LogDebug("Found show: {ResultTitle}", result.Title);
    }
  }
}
