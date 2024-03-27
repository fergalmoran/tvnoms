using Quartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Quartz.AspNetCore;
using TvNoms.Infrastructure.Jobs.Media.TheTvDb;

namespace TvNoms.Infrastructure.Jobs;

public static class ServiceCollectionExtensions {
  public static IServiceCollection AddScheduledJobs(this IServiceCollection services, IConfiguration config) {
    services.AddQuartz(q => {
      q.SchedulerId = "TVNoms-Scheduler";
      q.SchedulerName = "TVNoms Scheduler";

      q.AddJob<GetTrendingTvShowsJob>(opts => {
        opts
          .WithIdentity(new JobKey("GetTrendingTvShowsJob"))
          .StoreDurably();
      }).AddTrigger(opts => opts.ForJob(new JobKey("GetTrendingTvShowsJob"))
        .WithIdentity("GetTrendingTvShowsJob-Trigger")
        .WithDailyTimeIntervalSchedule(x =>
          x.WithInterval(10, IntervalUnit.Second)));
    });

    services.AddQuartzServer(options => {
      options.WaitForJobsToComplete = true;
    });

    return services;
  }
}
