﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvNoms.Core.Entities;

public abstract class BaseEntity : IEntity {
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public Guid Id { get; set; }

  [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
  public DateTime DateCreated { get; set; }

  [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
  public DateTime DateUpdated { get; set; }
}
