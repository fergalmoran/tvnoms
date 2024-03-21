namespace TvNoms.Server.Data.Models;

public record Client : BaseEntity {
  public Client() {
  }

  public string ConnectionId { get; set; } = default!;

  public DateTimeOffset ConnectionTime { get; set; }

  public string? IpAddress { get; set; }

  public string? DeviceId { get; set; }

  public Guid? UserId { get; set; }

  public User? User { get; set; }

  public string? UserAgent { get; set; }

  public bool Active { get; set; }
}
