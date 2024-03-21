using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvNoms.Server.Data.Models;

public interface IEntity {
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public Guid Id { get; set; }
}

public record BaseEntity : IEntity {
  public Guid Id { get; set; }

  [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
  public DateTime DateCreated { get; set; }

  [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
  public DateTime DateUpdated { get; set; }
}
