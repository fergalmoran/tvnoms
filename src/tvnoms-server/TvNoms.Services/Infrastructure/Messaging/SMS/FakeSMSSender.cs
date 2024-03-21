namespace TvNoms.Server.Services.Infrastructure.Messaging.SMS;

public class FakeSMSSender : ISmsSender {
  public Task SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }
}
