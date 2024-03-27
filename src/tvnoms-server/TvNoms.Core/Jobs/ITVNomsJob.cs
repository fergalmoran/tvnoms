using Quartz;

namespace TvNoms.Core.Jobs;

public interface ITVNomsJob : IJob {
  public string JobName { get; }
}
