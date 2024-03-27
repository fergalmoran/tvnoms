namespace TvNoms.Core.Entities;

using Microsoft.EntityFrameworkCore;

[Index(nameof(Name), IsUnique = true)]
public class Genre : BaseEntity {
  public string Name { get; set; } = default!;
  public string Description { get; set; } = default!;
}
