namespace TvNoms.Server.Services.Infrastructure.Messaging.SMS {
  public interface ISmsSender {
    Task SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default);
  }
}
