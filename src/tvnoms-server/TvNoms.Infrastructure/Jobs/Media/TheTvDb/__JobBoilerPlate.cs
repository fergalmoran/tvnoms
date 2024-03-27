using Microsoft.Extensions.Logging;
using Quartz;
using TvNoms.Core.Jobs;

namespace TvNoms.Infrastructure.Jobs.Media.TheTvDb;

public class __JobBoilerPlate : ITVNomsJob {
  private readonly ILogger _logger;
  public string JobName => "GetTrendingTvShowsJob";

  public __JobBoilerPlate(ILogger<__JobBoilerPlate> logger) {
    _logger = logger;
  }

  public async Task Execute(IJobExecutionContext context) {
    _logger.LogDebug("Starting __JobBoilerPlate");
  }
}
