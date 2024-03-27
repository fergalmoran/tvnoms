namespace TvNoms.Core.Entities;

public abstract class BaseTrackedEntity : BaseEntity {
  public ICollection<Genre> Genres { get; set; }
}
